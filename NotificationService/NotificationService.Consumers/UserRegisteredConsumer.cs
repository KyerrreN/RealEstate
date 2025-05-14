using MassTransit;
using NotificationService.Contracts;

namespace NotificationService.Consumers
{
    public class UserRegisteredConsumer : IConsumer<UserRegisteredEvent>
    {
        public Task Consume(ConsumeContext<UserRegisteredEvent> context)
        {
            var message = context.Message;

            Console.WriteLine($"User {message.FirstName} {message.LastName} with email {message.Email} has succesfully registered");

            return Task.CompletedTask;
        }
    }
}
