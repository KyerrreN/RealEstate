namespace ChatService.API.DTO
{
    public class CreateMessageDto
    {
        public Guid RealEstateId { get; set; }

        public string RecieverId { get; set; } = null!;

        public string Content { get; set; } = null!;
    }
}
