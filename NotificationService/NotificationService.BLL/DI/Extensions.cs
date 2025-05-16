using FluentEmail.MailKitSmtp;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.BLL.Interfaces;
using NotificationService.BLL.Services;

namespace NotificationService.BLL.DI
{
    public static class Extensions
    {
        public static void ConfigureBLL(this IServiceCollection services)
        {
            services
                .AddFluentEmail("default@example.com")
                .AddRazorRenderer()
                .AddMailKitSender(new SmtpClientOptions
                {
                    Server = "localhost",
                    Port = 25,
                    UseSsl = false,
                    RequiresAuthentication = false,
                    User = "User",
                    Password = "Password"
                });

            services.AddTransient<IEmailService, EmailService>();
        }
    }
}
