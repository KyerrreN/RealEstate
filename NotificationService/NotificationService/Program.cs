using NotificationService.BLL.DI;
using NotificationService.Consumers.DI;

namespace NotificationService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.RegisterConsumers();
            builder.Services.ConfigureBLL();

            var app = builder.Build();

            app.Run();
        }
    }
}
