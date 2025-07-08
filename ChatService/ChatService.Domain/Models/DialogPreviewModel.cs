namespace ChatService.Domain.Models
{
    public class DialogPreviewModel
    {
        public Guid RealEstateId { get; set; }
        public required string InterlocutorId { get; set; }
        public required string LastMessage { get; set; }
        public DateTime LastMessageTime { get; set; }

        public RealEstateModel? RealEstate { get; set; }
    }
}
