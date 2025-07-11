namespace ChatService.API.Options
{
    public class CorsOptions
    {
        public const string Position = "Cors";

        public required string[] Origins { get; set; }
    }
}
