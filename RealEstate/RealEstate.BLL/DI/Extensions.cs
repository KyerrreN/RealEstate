using Mapster;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NotificationService.Contracts;
using NotificationService.Contracts.Constants;
using RealEstate.BLL.Interfaces;
using RealEstate.BLL.Options;
using RealEstate.BLL.Services;
using RealEstate.DAL.DI;

namespace RealEstate.BLL.DI
{
    public static class Extensions
    {
        public static void RegisterBLL(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterDataAccess(configuration);
            services.AddMapster();

            var redisOptions = configuration
                .GetRequiredSection(RedisOptions.Position)
                .Get<RedisOptions>()
                ?? throw new InvalidOperationException($"Failed to bind {nameof(RedisOptions)} from settings");

            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IHistoryService, HistoryService>();
            services.AddScoped<IRealEstateService, RealEstateService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IUserService, UserService>();

            services.AddStackExchangeRedisCache(opt =>
            {
                opt.Configuration = redisOptions.ConnectionString;
                opt.InstanceName = redisOptions.InstanceName;
            });

            services.Configure<MassTransitOptions>(
                configuration.GetSection(MassTransitOptions.Option).Bind);

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    var options = context.GetRequiredService<IOptions<MassTransitOptions>>().Value;

                    cfg.Host(options.Host, "/", h =>
                    {
                        h.Username(options.Username);
                        h.Password(options.Password);
                    });

                    cfg.Message<UserRegisteredEvent>(e =>
                    {
                        e.SetEntityName(NotificationConstants.Exchange);
                    });

                    cfg.Message<RealEstateAddedEvent>(e =>
                    {
                        e.SetEntityName(NotificationConstants.Exchange);
                    });

                    cfg.Message<ReviewAddedEvent>(e =>
                    {
                        e.SetEntityName(NotificationConstants.Exchange);
                    });

                    cfg.Message<RealEstateDeletedEvent>(e =>
                    {
                        e.SetEntityName(NotificationConstants.Exchange);
                    });

                    cfg.Publish<UserRegisteredEvent>(e =>
                    {
                        e.ExchangeType = NotificationConstants.ExchangeType;
                    });

                    cfg.Publish<RealEstateAddedEvent>(e =>
                    {
                        e.ExchangeType = NotificationConstants.ExchangeType;
                    });

                    cfg.Publish<RealEstateDeletedEvent>(e =>
                    {
                        e.ExchangeType = NotificationConstants.ExchangeType;
                    });

                    cfg.Publish<ReviewAddedEvent>(e =>
                    {
                        e.ExchangeType = NotificationConstants.ExchangeType;
                    });
                });
            });
        }
    }
}
