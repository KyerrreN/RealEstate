using MassTransit;
using Microsoft.Extensions.Logging;
using NotificationService.BLL.Interfaces;
using NotificationService.Consumers.Constants;
using NotificationService.Contracts;

namespace NotificationService.Consumers
{
    public class RealEstateDeletedConsumer(
        IEmailService<RealEstateDeletedEvent> emailService,
        ILogger<RealEstateDeletedConsumer> logger) : IConsumer<RealEstateDeletedEvent>
    {
        public async Task Consume(ConsumeContext<RealEstateDeletedEvent> context)
        {
            var message = context.Message;

            logger.LogInformation("Message received. Sending message to {Email}", message.Email);

            await emailService.Send(
                message,
                TemplateConstants.RealEstateDeleted,
                SubjectConstants.RealEstateDeleted,
                context.CancellationToken);
        }
    }
}
