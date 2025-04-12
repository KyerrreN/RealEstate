using Microsoft.EntityFrameworkCore;
using RealEstate.DAL.Builders;
using RealEstate.DAL.Entities;
using RealEstate.DAL.Interfaces;
using RealEstate.DAL.Models;
using RealEstate.DAL.RequestParameters;

namespace RealEstate.DAL.Repositories
{
    public class RealEstateRepository
        (AppDbContext context,
        IRealEstateFilterBuilder filterBuilder)
        : BaseRepository<RealEstateEntity>(context), IRealEstateRepository
    {
        public async Task<PagedEntityModel<RealEstateEntity>> GetAllWithRequestParameters(
            RealEstateFilterParameters filters,
            CancellationToken ct)
        {
            //var realEstateItems = await Query
            //    .AsNoTracking()
            //    .Include(re => re.Owner)
            //    .FilterByPrice(filters.MinPrice, filters.MaxPrice)
            //    .FilterByEstateStatus(filters.EstateStatus)
            //    .FilterByEstateType(filters.EstateType)
            //    .FilterByCity(filters.City)
            //    .FilterByOwner(filters.OwnerId)
            //    .Sort(filters.OrderBy)
            //    .ToListAsync(ct);

            var realEstateQuery = Query
                .AsNoTracking()
                .Include(re => re.Owner)
                .AsQueryable();

            filterBuilder
                .SetEstateStatus(filters.EstateStatus)
                .SetCity(filters.City);

            realEstateQuery = filterBuilder.Build(realEstateQuery, ct);

            var count = await realEstateQuery.CountAsync(ct);
            var totalPages = (int)Math.Ceiling((double)count / filters.PageSize);

            var realEstateList = await realEstateQuery
                .Skip((filters.PageNumber - 1) * filters.PageSize)
                .Take(filters.PageSize)
                .ToListAsync(ct);

            return new PagedEntityModel<RealEstateEntity>
            {
                TotalCount = count,
                TotalPages = totalPages,
                Items = realEstateList,
                CurrentPage = filters.PageNumber,
            };
        }
    }
}
