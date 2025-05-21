using FluentEmail.MailKitSmtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.BLL.Constants;
using NotificationService.BLL.Interfaces;
using NotificationService.BLL.Options;
using NotificationService.BLL.Services;

namespace NotificationService.BLL.DI
{
    public static class Extensions
    {
        public static void ConfigureBLL(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailKitOptions>(
                configuration.GetSection(MailKitOptions.Option).Bind);

            var mailKitOptions = configuration
                .GetSection(MailKitOptions.Option)
                .Get<MailKitOptions>();

            services
                .AddFluentEmail("default@example.com")
                .AddRazorRenderer()
                .AddMailKitSender(new SmtpClientOptions
                {
                    Server = mailKitOptions!.Server,
                    Port = mailKitOptions.Port,
                    UseSsl = mailKitOptions.UseSsl,
                    RequiresAuthentication = mailKitOptions.RequiresAuthentication,
                    User = mailKitOptions.User,
                    Password = mailKitOptions.Password
                });

            services.AddScoped<IEmailService, EmailService>();
        }

        public static string CreatePathToEmailTemplate(this IWebHostEnvironment env, string directory)
        {
            return Path.Combine(env.ContentRootPath, directory);
        }
    }
}
