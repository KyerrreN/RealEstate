using ChatService.BLL.Interface;
using ChatService.BLL.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using СhatService.DAL.DI;

namespace ChatService.BLL.DI
{
    public static class ServiceExtensions
    {
        public static async Task RegisterBLL(this IServiceCollection services, IConfiguration configuration)
        {
            await services.RegisterDAL(configuration);

            services.AddScoped<IMessageService, MessageService>();
        }
    }
}
