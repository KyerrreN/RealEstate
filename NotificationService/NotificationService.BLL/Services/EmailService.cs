using FluentEmail.Core;
using Microsoft.AspNetCore.Hosting;
using NotificationService.BLL.Constants;
using NotificationService.BLL.DI;
using NotificationService.BLL.Interfaces;
using NotificationService.Contracts;

namespace NotificationService.BLL.Services
{
    public class EmailService(IFluentEmail email, IWebHostEnvironment env) : IEmailService
    {
        public async Task SendUserRegisterAsync(UserRegisteredEvent userMetadata, CancellationToken ct)
        {
            string templateFile = env.CreatePathToEmailTemplate(TemplateConstants.UserRegistered);

            var response = await email
                .To(userMetadata.Email)
                .Subject("Real Estate: New Account")
                .UsingTemplateFromFile(templateFile, userMetadata)
                .SendAsync();

            if (!response.Successful)
            {
                Console.WriteLine("Failed to send email");

                foreach(var error in response.ErrorMessages)
                {
                    Console.WriteLine($" - {error}");
                }
            }
            else
            {
                Console.WriteLine("Success");
            }
        }
    }
}
