namespace RealEstate.Presentation.Options
{
    public class SwaggerOptions
    {
        public const string Position = "Swagger";
        public const string Bearer = "Bearer";

        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Version { get; set; }
    }
}
