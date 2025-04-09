using RealEstate.DAL.Enums;

namespace RealEstate.BLL.Models
{
    public class HistoryModel : BaseModel
    {
        public Guid RealEstateId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CompletedAt { get; set; }
        public EstateAction EstateAction { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
