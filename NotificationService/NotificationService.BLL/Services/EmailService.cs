using FluentEmail.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NotificationService.BLL.Constants;
using NotificationService.BLL.DI;
using NotificationService.BLL.Interfaces;
using NotificationService.Contracts;

namespace NotificationService.BLL.Services
{
    public class EmailService
        (IFluentEmail email, 
        IWebHostEnvironment env,
        ILogger<EmailService> logger) : IEmailService
    {
        public async Task SendUserRegisterAsync(UserRegisteredEvent userMetadata, CancellationToken ct)
        {
            string templateFile = env.CreatePathToEmailTemplate(TemplateConstants.UserRegistered);

            var response = await email
                .To(userMetadata.Email)
                .Subject(SubjectConstants.UserRegistered)
                .UsingTemplateFromFile(templateFile, userMetadata)
                .SendAsync();

            if (!response.Successful)
            {
                logger.LogError("Message couldn't be sent");

                foreach(var error in response.ErrorMessages)
                {
                    logger.LogError(error);
                }
            }
            else
            {
                logger.LogInformation("Message sent successfully");
            }
        }
    }
}
