using MassTransit;
using NotificationService.BLL.Interfaces;
using NotificationService.Contracts;

namespace NotificationService.Consumers
{
    public class UserRegisteredConsumer(IEmailService emailService) : IConsumer<UserRegisteredEvent>
    {
        public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
        {
            var message = context.Message;

            await emailService.SendUserRegisterAsync(message, context.CancellationToken);
        }
    }
}
