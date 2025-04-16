namespace RealEstate.BLL.Models
{
    public class ReviewModel : BaseModel
    {
        public short Rating { get; set; }
        public string Comment { get; set; } = null!;
        public Guid AuthorId { get; set; }
        public Guid RecipientId { get; set; }
        public UserModel? Author { get; set; }
        public UserModel? Recipient { get; set; }
    }
}
