using RealEstate.DAL.Entities;
using RealEstate.Domain.Models;
using RealEstate.Domain.QueryParameters;

namespace RealEstate.DAL.Interfaces
{
    public interface IRealEstateRepository : IBaseRepository<RealEstateEntity>
    {
        Task<PagedEntityModel<RealEstateEntity>> GetAllWithRequestParameters(RealEstateFilterParameters filters, SortingParameters sorting, CancellationToken ct);
    }
}
