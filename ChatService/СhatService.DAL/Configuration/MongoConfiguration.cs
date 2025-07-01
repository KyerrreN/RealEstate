namespace СhatService.DAL.Configuration
{
    public class MongoConfiguration
    {
        public const string Position = "MongoDB";

        public required string ConnectionString { get; set; }
    }
}
