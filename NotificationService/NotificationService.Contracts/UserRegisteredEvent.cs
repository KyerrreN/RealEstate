namespace NotificationService.Contracts
{
    public class UserRegisteredEvent
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
