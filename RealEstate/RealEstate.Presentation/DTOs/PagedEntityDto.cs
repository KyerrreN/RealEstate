namespace RealEstate.Presentation.DTOs
{
    public record PagedEntityDto<T> where T : class
    {
        public required int TotalCount { get; init; } = 0;
        public required List<T> Items { get; init; }
        public required int CurrentPage { get; init; }
        public required int TotalPages { get; init; }
    }
}
