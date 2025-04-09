using RealEstate.DAL.Entities;
using RealEstate.DAL.Enums;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;

namespace RealEstate.DAL.Repositories.Extensions
{
    public static class RealEstateExtensions
    {
        public static IQueryable<RealEstateEntity> FilterByEstateStatus(this IQueryable<RealEstateEntity> entity, EstateStatus? estateStatus)
        {
            if (estateStatus is not null && estateStatus.HasValue)
                return entity.Where(re => estateStatus == re.EstateStatus);

            return entity;
        }
        public static IQueryable<RealEstateEntity> FilterByEstateType(this IQueryable<RealEstateEntity> entity, EstateType? estateType)
        {
            if (estateType is not null && estateType.HasValue)
                return entity.Where(re => estateType == re.EstateType);

            return entity;
        }
        public static IQueryable<RealEstateEntity> FilterByOwner(this IQueryable<RealEstateEntity> entity, Guid? ownerId)
        {
            if (ownerId is not null && ownerId.HasValue)
                return entity.Where(re => re.OwnerId == ownerId);

            return entity;
        }
        public static IQueryable<RealEstateEntity> FilterByPrice(this IQueryable<RealEstateEntity> entity, decimal? minPrice, decimal? maxPrice)
        {
            return entity.Where(re => re.Price >= minPrice && re.Price <= maxPrice);            
        }
        public static List<RealEstateEntity> ApplyPaging(this List<RealEstateEntity> entity, int pageNumber, int pageSize)
        {
            return entity.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }
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
