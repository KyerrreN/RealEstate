using MassTransit;
using Microsoft.Extensions.Logging;
using NotificationService.BLL.Interfaces;
using NotificationService.Contracts;

namespace NotificationService.Consumers
{
    public class RealEstateDeletedConsumer(
        IEmailService emailService,
        ILogger<RealEstateDeletedConsumer> logger) : IConsumer<RealEstateDeletedEvent>
    {
        public async Task Consume(ConsumeContext<RealEstateDeletedEvent> context)
        {
            var message = context.Message;

            logger.LogInformation("Message received. Sending message to {Email}", message.Email);

            await emailService.SendRealEstateDeletedAsync(message, context.CancellationToken);
        }
    }
}
