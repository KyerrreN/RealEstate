using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using СhatService.DAL.Configuration;

namespace СhatService.DAL.DI
{
    public static class ServiceExtensions
    {
        public static void RegisterDAL(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoSettings = configuration
                .GetRequiredSection(MongoConfiguration.Position)
                .Get<MongoConfiguration>();

            var mongoUrl = MongoUrl.Create(mongoSettings?.ConnectionString);
            var mongoClient = new MongoClient(mongoUrl);
            var database = mongoClient.GetDatabase(mongoUrl.DatabaseName);

            services.AddSingleton<IMongoDatabase>(database);
        }
    }
}
