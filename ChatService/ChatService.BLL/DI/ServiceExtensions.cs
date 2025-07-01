using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using СhatService.DAL.DI;

namespace ChatService.BLL.DI
{
    public static class ServiceExtensions
    {
        public static void RegisterBLL(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterDAL(configuration);
        }
    }
}
