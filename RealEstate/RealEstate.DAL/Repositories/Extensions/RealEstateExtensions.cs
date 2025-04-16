using RealEstate.DAL.Entities;
using System.Linq.Dynamic.Core;

namespace RealEstate.DAL.Repositories.Extensions
{
    public static class RealEstateExtensions
    {
        public static IQueryable<RealEstateEntity> Sort(this IQueryable<RealEstateEntity> benchmarks, string? orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return benchmarks.OrderBy(x => x.Id);

            var orderQuery = Utilities.CreateOrderQuery<RealEstateEntity>(orderByQueryString);

            if (string.IsNullOrEmpty(orderQuery))
            {
                return benchmarks.OrderBy(x => x.Id);
            }

            return benchmarks.OrderBy(orderQuery);
        }
    }
}
