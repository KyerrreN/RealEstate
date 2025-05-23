using NotificationService.BLL.DI;
using NotificationService.Consumers.DI;
using Serilog;

namespace NotificationService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

            builder.Host.UseSerilog();

            builder.Services.RegisterConsumers(builder.Configuration);
            builder.Services.ConfigureBLL(builder.Configuration);

            var app = builder.Build();

            app.Run();
        }
    }
}
