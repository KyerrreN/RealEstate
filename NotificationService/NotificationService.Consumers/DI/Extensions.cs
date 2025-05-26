using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NotificationService.Consumers.Options;
using NotificationService.Contracts.Constants;

namespace NotificationService.Consumers.DI
{
    public static class Extensions
    {
        public static void RegisterConsumers(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MassTransitOptions>(
                configuration.GetSection(MassTransitOptions.Option).Bind);

            services.AddMassTransit(x =>
            {
                x.AddConsumer<UserRegisteredConsumer>();
                x.AddConsumer<RealEstateAddedConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    var options = context.GetRequiredService<IOptions<MassTransitOptions>>().Value;

                    cfg.Host(options.Host, h =>
                    {
                        h.Username(options.Username);
                        h.Password(options.Password);
                    });

                    cfg.ReceiveEndpoint("user-queue", e =>
                    {
                        e.Bind(NotificationConstants.Exchange, s =>
                        {
                            s.RoutingKey = NotificationConstants.UserRoutingKey;
                            s.ExchangeType = NotificationConstants.ExchangeType;
                        });

                        e.ConfigureConsumer<UserRegisteredConsumer>(context);
                    });

                    cfg.ReceiveEndpoint("real-estate-queue", e =>
                    {
                        e.Bind(NotificationConstants.Exchange, s =>
                        {
                            s.RoutingKey = NotificationConstants.RealEstateAddedRoutingKey;
                            s.ExchangeType = NotificationConstants.ExchangeType;
                        });

                        e.ConfigureConsumer<RealEstateAddedConsumer>(context);
                    });
                });
            });
        }
    }
}
