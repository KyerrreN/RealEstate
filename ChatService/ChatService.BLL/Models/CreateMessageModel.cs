namespace ChatService.BLL.Models
{
    public class CreateMessageModel
    {
        public Guid RealEstateId { get; set; }

        public string SenderId { get; set; } = null!;

        public string RecieverId { get; set; } = null!;

        public string Content { get; set; } = null!;
    }
}
