using Mapster;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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

            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IHistoryService, HistoryService>();
            services.AddScoped<IRealEstateService, RealEstateService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IUserService, UserService>();

            services.Configure<MassTransitOptions>(
                configuration.GetSection(MassTransitOptions.Option).Bind);

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    var options = context.GetRequiredService<IOptions<MassTransitOptions>>().Value;

                    cfg.Host(options.Host, h =>
                    {
                        h.Username(options.Username);
                        h.Password(options.Password);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });
        }
    }
}
