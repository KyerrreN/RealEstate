using ChatService.API.Hubs;
using ChatService.BLL.DI;
using ChatService.BLL.Grpc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
            {
                opt.Authority = "https://test-realestate.eu.auth0.com/";
                opt.Audience = "https://realestate.com/api";
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
                    .AllowAnyMethod();
            });
            app.UseAuthentication();    
            app.UseAuthorization();

            app.MapMessageEndpoints();
            app.MapHub<ChatHub>("/chathub");

            app.Run();
        }
    }
}
