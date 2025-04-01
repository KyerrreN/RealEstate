namespace RealEstate.DAL.Entities
{
    public class Review
    {
        public Guid Id { get; set; } // PK
        public short Rating { get; set; }
        public string Comment { get; set; } = null!;
        public Guid AuthorId { get; set; } // FK
        public Guid RecipientId { get; set; } // FK
    }
}
