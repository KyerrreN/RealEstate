using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RealEstate.DAL.Builders;
using RealEstate.DAL.Entities;
using RealEstate.DAL.Interfaces;
using RealEstate.DAL.Repositories.Extensions;
using RealEstate.Domain.Models;
using RealEstate.Domain.QueryParameters;
using System.Text.Json;

namespace RealEstate.DAL.Repositories
{
    public class RealEstateRepository
        (AppDbContext context,
        IRealEstateFilterBuilder filterBuilder)
        : BaseRepository<RealEstateEntity>(context), IRealEstateRepository
    {
        public async Task<PagedEntityModel<RealEstateEntity>> GetAllWithRequestParameters(
            RealEstateFilterParameters filters,
            SortingParameters sorting,
            CancellationToken ct)
        {
            var realEstateQuery = Query
                .AsNoTracking()
                .Include(re => re.Owner)
                .AsQueryable();

            filterBuilder
                .SetEstateStatus(filters.EstateStatus)
                .SetEstateType(filters.EstateType)
                .SetOwner(filters.OwnerId)
                .SetCity(filters.City)
                .SetPrice(filters.MinPrice, filters.MaxPrice);
            
            realEstateQuery = filterBuilder.Build(realEstateQuery, ct);
            realEstateQuery.Sort(sorting.OrderBy);

            var realEstateList = await Utilities.ToPagedEntityModelAsync(filters.PageNumber, filters.PageSize, realEstateQuery, ct);

            return realEstateList;
        }

        public async Task<RealEstateEntity?> FindByIdWithUserAsync(Guid id, CancellationToken ct)
        {
            return await Query
                .Include(re => re.Owner)
                .FirstOrDefaultAsync(re => re.Id == id, cancellationToken: ct);
        }

        public async Task<List<RealEstateEntity>> GetByIdsAsync(Guid[] ids, CancellationToken ct)
        {
            return await Query
                .Where(re => ids.Contains(re.Id))
                .ToListAsync(ct);
        }
    }
}
