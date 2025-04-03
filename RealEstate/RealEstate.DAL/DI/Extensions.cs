using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstate.DAL.Configurations;
using RealEstate.DAL.Enums;
using RealEstate.DAL.Interceptors;
using RealEstate.DAL.Repositories;

namespace RealEstate.DAL.DI
{
    public static class Extensions
    {
        public static void RegisterDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            var dbContextConfig = new DbContextConfiguration
            {
                ConnectionString = configuration.GetSection("PostgreSQL").Get<DbContextConfiguration>()?.ConnectionString
            };

            if (dbContextConfig.ConnectionString is null)
            {
                throw new ArgumentException("Connection String is not configured");
            }
            else
            {
                services.AddDbContext<AppDbContext>(opt =>
                    opt.UseNpgsql(dbContextConfig.ConnectionString, o =>
                    {
                        o.MapEnum<EstateAction>(nameof(EstateAction))
                        .MapEnum<EstateStatus>(nameof(EstateStatus))
                        .MapEnum<EstateType>(nameof(EstateType));
                    }).AddInterceptors(new TimeStampInterceptor()));
            }
        }
    }
}
