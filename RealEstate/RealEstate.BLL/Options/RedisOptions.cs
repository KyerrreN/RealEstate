namespace RealEstate.BLL.Options
{
    public class RedisOptions
    {
        public const string Position = "Redis";

        public required string ConnectionString { get; set; }
        public required string InstanceName { get; set; }
    }
}
