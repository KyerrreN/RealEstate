using ChatService.BLL.DI;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ChatService.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            await builder.Services.RegisterBLL(builder.Configuration);

            // TEMP, will be replaced with Auth0
            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes("super_secret_key_123_123212313213"))
                    };
                });
            builder.Services.AddAuthorization();

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
                    .AllowAnyMethod();
            });
            app.UseAuthentication();    
            app.UseAuthorization();

            app.MapMessageEndpoints();

            app.Run();
        }
    }
}
