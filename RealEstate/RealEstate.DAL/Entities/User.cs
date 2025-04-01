namespace RealEstate.DAL.Entities
{
    public class User
    {
        public Guid Id { get; set; } // PK
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }

        // nav props
        public ICollection<Review> Reviews { get; } = [];
        public ICollection<History> Histories { get; } = [];
        public ICollection<RealEstate> RealEstates { get; } = [];
    }
}
