using MassTransit;
using Microsoft.Extensions.Logging;
using NotificationService.BLL.Interfaces;
using NotificationService.Consumers.Constants;
using NotificationService.Contracts;

namespace NotificationService.Consumers
{
    public class UserRegisteredConsumer
        (IEmailService<UserRegisteredEvent> emailService,
        ILogger<UserRegisteredConsumer> logger) : IConsumer<UserRegisteredEvent>
    {
        public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
        {
            var message = context.Message;

            logger.LogInformation("Message from main service received. Sending email to {Email}", message.Email);

            await emailService.Send(
                message, 
                TemplateConstants.UserRegistered,
                SubjectConstants.UserRegistered,
                context.CancellationToken);
        }
    }
}
