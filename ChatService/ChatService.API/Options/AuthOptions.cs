namespace ChatService.API.Options
{
    public class AuthOptions
    {
        public const string Position = "Auth";

        public required string Domain { get; set; }
        public required string Audience { get; set; }
    }
}
