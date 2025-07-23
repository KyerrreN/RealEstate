using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.OpenApi.Models;
using RealEstate.BLL.DI;
using RealEstate.BLL.Grpc.Interceptors;
using RealEstate.BLL.Grpc.Services;
using RealEstate.Presentation.Mapping;
using RealEstate.Presentation.Middleware;
using RealEstate.Presentation.Options;
using Serilog;

namespace RealEstate.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var authSettings = builder.Configuration.GetRequiredOptions<AuthOptions>(AuthOptions.Position);
            var swaggerSettings = builder.Configuration.GetRequiredOptions<SwaggerOptions>(SwaggerOptions.Position);
            var corsSettings = builder.Configuration.GetRequiredOptions<CorsOptions>(CorsOptions.Position);

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(8080, o => o.Protocols = HttpProtocols.Http1);
                options.ListenAnyIP(8081, o => o.Protocols = HttpProtocols.Http2);
            });

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

            builder.Host.UseSerilog();

            MapsterConfig.RegisterMappings();
            builder.Services.AddValidatorsFromAssemblyContaining<Program>();

            builder.Services.RegisterBLL(builder.Configuration);

            builder.Services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = swaggerSettings.Title,
                    Version = swaggerSettings.Version,
                    Description = swaggerSettings.Description
                });

                s.AddSecurityDefinition(SwaggerOptions.Bearer, new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Place to add JWT with bearer",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = SwaggerOptions.Bearer
                });

                s.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = SwaggerOptions.Bearer
                            },
                            Name = SwaggerOptions.Bearer
                        },
                        []
                    }
                });
            });

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.Authority = authSettings.Domain;
                    opt.Audience = authSettings.Audience;
                });

            builder.Services.AddCors(opt =>
            {
                opt.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins(corsSettings.Origins)
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            builder.Services.AddGrpc(opt =>
            {
                opt.Interceptors.Add<GrpcLoggingInterceptor>();
            });

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapGrpcService<RealEstateGrpcService>();

            app.Run();
        }
    }
}
