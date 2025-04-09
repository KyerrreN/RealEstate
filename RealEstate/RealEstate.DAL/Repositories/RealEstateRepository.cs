using Microsoft.EntityFrameworkCore;
using RealEstate.DAL.Entities;
using RealEstate.DAL.Interfaces;
using RealEstate.DAL.Models;
using RealEstate.DAL.Repositories.Extensions;
using RealEstate.DAL.RequestParameters;

namespace RealEstate.DAL.Repositories
{
    public class RealEstateRepository(AppDbContext context) : BaseRepository<RealEstateEntity>(context), IRealEstateRepository
    {
        public async Task<PagedEntityModel<RealEstateEntity>> GetAllWithRequestParameters(RealEstateFilterParameters filters, CancellationToken ct)
        {
            var realEstateItems = await Query
                .AsNoTracking()
                .Include(re => re.Owner)
                .FilterByPrice(filters.MinPrice, filters.MaxPrice)
                .FilterByEstateStatus(filters.EstateStatus)
                .FilterByEstateType(filters.EstateType)
                .FilterByCity(filters.City)
                .FilterByOwner(filters.OwnerId)
                .Sort(filters.OrderBy)
                .ToListAsync(ct);

            var totalCount = realEstateItems.Count;

            var totalPages = (int)Math.Ceiling((double)totalCount / filters.PageSize);

            var pagedEntity = new PagedEntityModel<RealEstateEntity>
            {
                Items = realEstateItems.ApplyPaging(filters.PageNumber, filters.PageSize),
                CurrentPage = filters.PageNumber,
                TotalCount = totalCount,
                TotalPages = totalPages
            };

            return pagedEntity;
        }
    }
}
