namespace ChatService.BLL.Models
{
    public class MessageModel
    {
        public string? Id { get; set; }

        public Guid RealEstateId { get; set; }

        public string SenderId { get; set; } = null!;

        public string RecieverId { get; set; } = null!;

        public string Content { get; set; } = null!;

        public DateTime SentAt { get; set; }
    }
}
