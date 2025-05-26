namespace NotificationService.Contracts
{
    public class ReviewAddedEvent : BaseEvent
    {
        public required short Rating { get; set; }
        public required string Comment { get; set; }
        public required string AuthorFirstName { get; set; }
        public required string AuthorLastName { get; set; }
    }
}
