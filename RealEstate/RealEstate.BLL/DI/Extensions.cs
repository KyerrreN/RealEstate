using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstate.DAL.DI;

namespace RealEstate.BLL.DI
{
    public static class Extensions
    {
        public static void RegisterBLL(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterDataAccess(configuration);
        }
    }
}
