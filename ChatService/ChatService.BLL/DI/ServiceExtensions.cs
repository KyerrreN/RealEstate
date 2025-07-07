using ChatService.BLL.Grpc;
using ChatService.BLL.Interface;
using ChatService.BLL.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstate.Grpc;
using СhatService.DAL.DI;

namespace ChatService.BLL.DI
{
    public static class ServiceExtensions
    {
        public static async Task RegisterBLL(this IServiceCollection services, IConfiguration configuration)
        {
            await services.RegisterDAL(configuration);

            services.AddScoped<IMessageService, MessageService>();


            services.AddGrpcClient<RealEstateService.RealEstateServiceClient>(opt =>
            {
                opt.Address = new Uri("https://localhost:7283");
            });
            services.AddScoped<RealEstateGrpcClient>();
        }
    }
}
