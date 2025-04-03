namespace RealEstate.DAL.Configurations
{
    public record DbContextConfiguration
    {
        public string? ConnectionString { get; init; }
    }
}
