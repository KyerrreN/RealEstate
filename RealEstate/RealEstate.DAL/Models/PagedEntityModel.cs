namespace RealEstate.DAL.Models
{
    public class PagedEntityModel<T> where T : class
    {
        public int TotalCount { get; set; } = 0;
        public List<T> Items { get; set; } = [];
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
