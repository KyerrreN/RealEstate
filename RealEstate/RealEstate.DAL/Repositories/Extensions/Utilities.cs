using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Models;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;

namespace RealEstate.DAL.Repositories.Extensions
{
    public static class Utilities
    {
        public static string CreateOrderQuery<T>(string orderByQueryString)
        {
            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();

            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                {
                    continue;
                }

                var propertyFromQueryName = param.Split(" ")[0];
                var objectProperty = propertyInfos.FirstOrDefault(prop =>
                    prop.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty == null)
                {
                    continue;
                }

                var direction = param.EndsWith(" desc") ? "descending" : "ascending";

                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {direction}, ");
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

            return orderQuery;
        }
        public static List<T> ApplyPaging<T>(this List<T> entity, int pageNumber, int pageSize)
        {
            return entity.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        public static async Task<List<T>> ApplyPagingAndToListAsync<T>(this IQueryable<T> entities,
            int pageNumber,
            int pageSize,
            CancellationToken ct)
        {
            return await entities.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        }
        public static async Task<PagedEntityModel<T>> ToPagedEntityModelAsync<T>(int pageNumber, int pageSize, IQueryable<T> items, CancellationToken ct) where T : class
        {
            var totalCount = await items.CountAsync(ct);
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            var pagedItems = await items.ApplyPagingAndToListAsync(pageNumber, pageSize, ct);

            return new PagedEntityModel<T>
            {
                TotalCount = totalCount,
                Items = pagedItems,
                CurrentPage = pageNumber,
                TotalPages = totalPages
            };
        }
        public static PagedEntityModel<T> ToPagedEntityModel<T>(int pageNumber, int pageSize, List<T> items) where T : class
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
