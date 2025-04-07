using RealEstate.DAL.Entities;
using RealEstate.DAL.Enums;

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
        public static IQueryable<RealEstateEntity> ApplyPaging(this IQueryable<RealEstateEntity> entity, int pageNumber, int pageSize)
        {
            return entity.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
    }
}
