namespace NotificationService.Contracts
{
    public class RealEstateDeletedEvent : BaseEvent
    {
        public required string Title { get; set; }
        public required string Address { get; set; }
    }
}
