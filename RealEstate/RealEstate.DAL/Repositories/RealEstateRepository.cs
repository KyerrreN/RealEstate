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

            var pagedEntity = PagedEntityModel<RealEstateEntity>.ToPagedEntityModel(filters.PageNumber, filters.PageSize, realEstateItems);

            return pagedEntity;
        }
    }
}
