namespace RealEstate.BLL.Models
{
    public class UserModel : BaseModel
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string Auth0Id { get; set; } = null!;
    }
}
