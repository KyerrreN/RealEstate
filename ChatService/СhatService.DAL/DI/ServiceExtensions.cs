using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using СhatService.DAL.Configuration;
using СhatService.DAL.Documents;
using СhatService.DAL.Interface;
using СhatService.DAL.Repository;

namespace СhatService.DAL.DI
{
    public static class ServiceExtensions
    {
        public static async Task RegisterDAL(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoSettings = configuration
                .GetRequiredSection(MongoConfiguration.Position)
                .Get<MongoConfiguration>()
                ?? throw new InvalidOperationException("Couldn't find MongoDB configuration");

            var mongoUrl = MongoUrl.Create(mongoSettings.ConnectionString);
            var mongoClient = new MongoClient(mongoUrl);
            var database = mongoClient.GetDatabase(mongoUrl.DatabaseName);

            services.AddSingleton<IMongoDatabase>(database);
            services.AddScoped<IMessageRepository, MessageRepository>();

            await InitializeMongo(database, mongoSettings);
        }

        private static async Task InitializeMongo(IMongoDatabase db, MongoConfiguration mongoSettings)
        {
            var existingCollection = await db.ListCollectionNames().ToListAsync();

            if (!existingCollection.Contains(mongoSettings.Collection))
            {
                await db.CreateCollectionAsync(mongoSettings.Collection);
            }

            var messagesCollection = db.GetCollection<Message>(mongoSettings.Collection);

            var indexKeys = Builders<Message>.IndexKeys
                .Ascending(m => m.RealEstateId);

            var indexModel = new CreateIndexModel<Message>(indexKeys, new CreateIndexOptions
            {
                Name = "IX_RealEstateId"
            });

            await messagesCollection.Indexes.CreateOneAsync(indexModel);
        }
    }
}
