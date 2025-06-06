﻿using MassTransit;
using Microsoft.Extensions.Logging;
using NotificationService.BLL.Interfaces;
using NotificationService.Consumers.Constants;
using NotificationService.Contracts;

namespace NotificationService.Consumers
{
    public class RealEstateAddedConsumer
        (IEmailService<RealEstateAddedEvent> emailService,
        ILogger<RealEstateAddedConsumer> logger) : IConsumer<RealEstateAddedEvent>
    {
        public async Task Consume(ConsumeContext<RealEstateAddedEvent> context)
        {
            var message = context.Message;

            logger.LogInformation("Message received. Sending message to {Email}", message.Email);

            await emailService.Send(
                message, 
                TemplateConstants.RealEstateAdded, 
                SubjectConstants.RealEstateAdded, 
                context.CancellationToken);
        }
    }
}
