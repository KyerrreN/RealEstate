namespace RealEstate.DAL.Entities
{
    public class UserEntity : BaseEntity
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }

        public ICollection<ReviewEntity> Reviews { get; } = [];
        public ICollection<HistoryEntity> Histories { get; } = [];
        public ICollection<RealEstateEntity> RealEstates { get; } = [];
    }
}
