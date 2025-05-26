namespace NotificationService.Contracts
{
    public class RealEstateAddedEvent
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }
        public required string Address { get; set; }
        public required string EstateType { get; set; }
        public required string EstateStatus { get; set; }
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }
}
