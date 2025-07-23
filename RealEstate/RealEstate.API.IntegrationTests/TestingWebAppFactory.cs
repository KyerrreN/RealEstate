using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RealEstate.DAL.Repositories;
using MassTransit;
using Microsoft.AspNetCore.Authentication;

namespace RealEstate.API.IntegrationTests
{
    public class TestingWebAppFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddAuthentication("TestScheme")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", options => { });

                services.PostConfigure<AuthenticationOptions>(options =>
                {
                    options.DefaultAuthenticateScheme = "TestScheme";
                    options.DefaultChallengeScheme = "TestScheme";
                });

                ConfigureInMemoryDatabase(services);
                ConfigureRabbitMQConnection(services);

                services.AddDistributedMemoryCache();

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

        private static void ConfigureInMemoryDatabase(IServiceCollection services)
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

        private static void ConfigureRabbitMQConnection(IServiceCollection services)
        {
            for (int i = services.Count - 1; i >= 0; i--)
            {
                var serviceType = services[i].ServiceType;

                if (serviceType.Namespace is string ns && ns.StartsWith("MassTransit"))
                {
                    services.RemoveAt(i);
                }
            }

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
