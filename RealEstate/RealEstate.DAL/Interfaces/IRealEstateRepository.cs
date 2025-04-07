using RealEstate.DAL.Entities;
using RealEstate.DAL.Models;
using RealEstate.DAL.RequestParameters;

namespace RealEstate.DAL.Interfaces
{
    public interface IRealEstateRepository : IBaseRepository<RealEstateEntity>
    {
        Task<PagedEntityModel<RealEstateEntity>> GetAllWithRequestParameters(RealEstateFilterParameters filters, CancellationToken ct);
    }
}
