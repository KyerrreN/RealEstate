namespace NotificationService.Consumers.Options
{
    public class MassTransitOptions
    {
        public const string Option = "MassTransit";
        public string Host { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
