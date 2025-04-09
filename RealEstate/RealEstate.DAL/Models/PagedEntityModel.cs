using RealEstate.DAL.Repositories.Extensions;

namespace RealEstate.DAL.Models
{
    public class PagedEntityModel<T> where T : class
    {
        public int TotalCount { get; set; } = 0;
        public List<T> Items { get; set; } = [];
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public static PagedEntityModel<T> ToPagedEntityModel(int pageNumber, int pageSize, List<T> items)
        {
            var totalCount = items.Count;
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            var pagedItems = items.ApplyPaging(pageNumber, pageSize);

            return new PagedEntityModel<T>
            {
                TotalCount = totalCount,
                Items = pagedItems,
                CurrentPage = pageNumber,
                TotalPages = totalPages
            };
        }
    }
}
