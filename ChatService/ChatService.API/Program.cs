using ChatService.API.Constants;
using ChatService.API.Hubs;
using ChatService.API.Options;
using ChatService.BLL.DI;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ChatService.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            await builder.Services.RegisterBLL(builder.Configuration);

            var authOptions = builder.Configuration.GetRequiredSection(AuthOptions.Position).Get<AuthOptions>()
                ?? throw new InvalidOperationException($"Failed to bind {nameof(AuthOptions)} from position: {AuthOptions.Position}");
            var corsOptions = builder.Configuration.GetRequiredSection(CorsOptions.Position).Get<CorsOptions>()
                ?? throw new InvalidOperationException($"Failed to bind {nameof(CorsOptions)} from position: {CorsOptions.Position}");

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.Authority = authOptions.Domain;
                    opt.Audience = authOptions.Audience;

                    opt.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments(ApiConstants.RouteHub)))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
            builder.Services.AddAuthorization();

            builder.Services.AddSignalR();

            builder.Services.AddCors();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseCors(opt =>
            {
                opt.WithOrigins("http://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapMessageEndpoints();
            app.MapHub<ChatHub>(ApiConstants.RouteHub);

            app.Run();
        }
    }
}
