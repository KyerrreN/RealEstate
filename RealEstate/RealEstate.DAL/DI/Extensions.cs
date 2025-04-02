using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RealEstate.DAL.Enums;
using RealEstate.DAL.Repositories;
using RealEstate.DAL.Interceptors;

namespace RealEstate.DAL.DI
{
    public static class Extensions
    {
        public static void RegisterDataAccess(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<RepositoryContext>(opt =>
                opt.UseNpgsql(connectionString, o =>
                {
                    o.MapEnum<EstateAction>(nameof(EstateAction))
                     .MapEnum<EstateStatus>(nameof(EstateStatus))
                     .MapEnum<EstateType>(nameof(EstateType));
                }).AddInterceptors(new TimeStampInterceptor()));
        }
    }
}
