using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RealEstate.DAL.Repositories;
using MassTransit;

namespace RealEstate.API.IntegrationTests
{
    public class TestingWebAppFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                ConfigureInMemoryDatabase(services);
                ConfigureRabbitMQConnection(services);

                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();

                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<AppDbContext>();

                try
                {
                    db.Database.EnsureCreated();
                }
                catch (Exception)
                {
                    throw;
                }
            });
        }

        private void ConfigureInMemoryDatabase(IServiceCollection services)
        {
            var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

            if (descriptor is not null)
                services.Remove(descriptor);

            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryRealEstateTest");
                options.UseInternalServiceProvider(serviceProvider);
            });
        }

        private void ConfigureRabbitMQConnection(IServiceCollection services)
        {
            var massTransitDescriptors = services.Where(d =>
                d.ServiceType.Namespace != null &&
                d.ServiceType.Namespace.StartsWith("MassTransit")).ToList();

            foreach (var d in massTransitDescriptors)
                services.Remove(d);

            var healthCheckDescriptor = services.SingleOrDefault(d =>
                d.ServiceType.Name == "IHealthCheck" &&
                d.ImplementationInstance?.GetType().FullName?.Contains("MassTransit") == true);

            if (healthCheckDescriptor is not null)
                services.Remove(healthCheckDescriptor);

            services.AddMassTransit(x =>
            {
                x.UsingInMemory((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                });
            });
        }
    }
}
