namespace NotificationService.BLL.Options
{
    public class MailKitOptions
    {
        public const string Option = "MailKit";

        public string Server { get; set; } = string.Empty;
        public int Port { get; set; }
        public bool UseSsl { get; set; } = false;
        public bool RequiresAuthentication { get; set; } = false;
        public string User { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
