using MassTransit;
using Microsoft.Extensions.Logging;
using NotificationService.BLL.Interfaces;
using NotificationService.Contracts;

namespace NotificationService.Consumers
{
    public class UserRegisteredConsumer
        (IEmailService emailService,
        ILogger<UserRegisteredConsumer> logger) : IConsumer<UserRegisteredEvent>
    {
        public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
        {
            var message = context.Message;

            logger.LogInformation("Message from main service recieved. Sending email to {Email}", message.Email);

            await emailService.SendUserRegisterAsync(message, context.CancellationToken);
        }
    }
}
