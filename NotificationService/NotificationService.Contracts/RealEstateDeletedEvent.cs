namespace NotificationService.Contracts
{
    public class RealEstateDeletedEvent
    {
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Title { get; set; }
        public required string Address { get; set; }
    }
}
