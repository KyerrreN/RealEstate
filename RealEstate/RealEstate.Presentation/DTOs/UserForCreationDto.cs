﻿namespace RealEstate.Presentation.DTOs
{
    public record UserForCreationDto
    {
        public string FirstName { get; init; } = null!;
        public string LastName { get; init; } = null!;
        public string Email { get; init; } = null!;
        public string? Phone { get; init; }
    }
}
