namespace RealEstate.DAL.Options
{
    public class AppDbContextOptions
    {
        public const string Option = "PostgreSQL";
        public string? ConnectionString { get; set; }
    }
}
