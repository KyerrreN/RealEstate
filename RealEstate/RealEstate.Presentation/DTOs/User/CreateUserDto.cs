namespace RealEstate.Presentation.DTOs.User
{
    public record CreateUserDto
    {
        public string FirstName { get; init; } = null!;
        public string LastName { get; init; } = null!;
        public string Email { get; init; } = null!;
        public string? Phone { get; init; }
    }
}
