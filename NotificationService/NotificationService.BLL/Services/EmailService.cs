using FluentEmail.Core;
using FluentEmail.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NotificationService.BLL.DI;
using NotificationService.BLL.Interfaces;
using NotificationService.Contracts;

namespace NotificationService.BLL.Services
{
    public class EmailService<T>
        (IFluentEmail email, 
        IWebHostEnvironment env,
        ILogger<EmailService<T>> logger) : IEmailService<T> where T : BaseEvent
    {
        public async Task Send(T data, string path, string subject, CancellationToken ct)
        {
            string templateFile = env.CreatePathToEmailTemplate(path);

            var response = await email
                .To(data.Email)
                .Subject(subject)
                .UsingTemplateFromFile(templateFile, data)
                .SendAsync(ct);

            ProcessResponseSuccess(response);
        }

        private void ProcessResponseSuccess(SendResponse response)
        {
            if (!response.Successful)
            {
                logger.LogError("Message couldn't be sent");

                foreach (var error in response.ErrorMessages)
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
