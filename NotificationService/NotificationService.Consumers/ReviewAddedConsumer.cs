using MassTransit;
using Microsoft.Extensions.Logging;
using NotificationService.BLL.Interfaces;
using NotificationService.Consumers.Constants;
using NotificationService.Contracts;

namespace NotificationService.Consumers
{
    public class ReviewAddedConsumer(
        IEmailService<ReviewAddedEvent> emailService,
        ILogger<ReviewAddedConsumer> logger) : IConsumer<ReviewAddedEvent>
    {
        public async Task Consume(ConsumeContext<ReviewAddedEvent> context)
        {
            var message = context.Message;

            logger.LogInformation("Message received. Sending message to {Email}", message.Email);

            await emailService.Send(
                message, 
                TemplateConstants.ReviewAdded,
                SubjectConstants.ReviewAdded,
                context.CancellationToken);
        }
    }
}
