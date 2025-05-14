using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace NotificationService.Consumers.DI
{
    public static class Extensions
    {
        public static void RegisterConsumers(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<UserRegisteredConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });
        }
    }
}
