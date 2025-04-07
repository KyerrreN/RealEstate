namespace RealEstate.Presentation.DTOs
{
    public class PagedEntityDto<T> where T : class
    {
        public required int TotalCount { get; set; } = 0;
        public required List<T> Items { get; set; }
        public required int CurrentPage { get; set; }
        public required int TotalPages { get; set; }
    }
}
