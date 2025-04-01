namespace RealEstate.DAL.Entities
{
    public class User
    {
        public Guid Id { get; set; } // PK
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
    }
}
