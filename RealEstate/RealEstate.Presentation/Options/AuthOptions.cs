namespace RealEstate.Presentation.Options
{
    public record AuthOptions
    {
        public const string Position = "Auth0";

        public required string Domain { get; set; }
        public required string Audience { get; set; }
        public string? ClientSecret { get; set; }
    }
}
