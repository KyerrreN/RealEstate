namespace RealEstate.DAL.Entities
{
    public class UserEntity : BaseEntity
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string Auth0Id { get; set; } = null!;

        public ICollection<ReviewEntity> AuthoredReviews { get; } = [];
        public ICollection<ReviewEntity> ReceivedReviews { get; } = [];
        public ICollection<HistoryEntity> Histories { get; } = [];
        public ICollection<RealEstateEntity> RealEstates { get; } = [];
        public ICollection<BookingEntity> Bookings { get; set; } = [];
    }
}
