﻿namespace RealEstate.Domain.Models
{
    public record ErrorModel
    {
        public required int StatusCode { get; init; }
        public required string Message { get; init; }
    }
}
