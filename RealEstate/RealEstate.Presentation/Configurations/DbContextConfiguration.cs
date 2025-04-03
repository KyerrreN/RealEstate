namespace RealEstate.Presentation.Configurations
{
    public record DbContextConfiguration
    {
        public string? ConnectionString { get; init; }
    }
}
