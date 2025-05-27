namespace NotificationService.Contracts
{
    public class RealEstateAddedEvent : BaseEvent
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }
        public required string Address { get; set; }
        public required string EstateType { get; set; }
        public required string EstateStatus { get; set; }
    }
}
