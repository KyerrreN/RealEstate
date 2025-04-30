using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RealEstate.DAL.Builders;
using RealEstate.DAL.Interceptors;
using RealEstate.DAL.Interfaces;
using RealEstate.DAL.Options;
using RealEstate.DAL.Repositories;
using RealEstate.DAL.Transactions;
using RealEstate.Domain.Enums;
using RealEstate.Domain.Interfaces;
using RealEstate.Domain.Services;

namespace RealEstate.DAL.DI
{
    public static class Extensions
    {
        public static void RegisterDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            var isTestingEnvironment = Environment.GetEnvironmentVariable("INTEGRATION_TESTS") == "true";

            if (!isTestingEnvironment)
            {
                services.Configure<AppDbContextOptions>(configuration.GetSection(AppDbContextOptions.Option).Bind);

                services.AddDbContext<AppDbContext>((serviceProvider, opt) =>
                {
                    var dbContextOptions = serviceProvider.GetRequiredService<IOptions<AppDbContextOptions>>().Value;

                    if (string.IsNullOrWhiteSpace(dbContextOptions.ConnectionString))
                    {
                        throw new ArgumentException("Connection String is not configured");
                    }

                    opt.UseNpgsql(dbContextOptions.ConnectionString, o =>
                    {
                        o.MapEnum<EstateAction>(nameof(EstateAction))
                         .MapEnum<EstateStatus>(nameof(EstateStatus))
                         .MapEnum<EstateType>(nameof(EstateType));
                    })
                    .AddInterceptors(new TimeStampInterceptor());
                });
            }

            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IHistoryRepository, HistoryRepository>();
            services.AddScoped<IRealEstateRepository, RealEstateRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            services.AddScoped<IRealEstateFilterBuilder, RealEstateFilterBuilder>();

            services.AddScoped<ITransactionManager, TransactionManager>();

            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        }
    }
}
