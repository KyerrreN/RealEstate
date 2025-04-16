using RealEstate.BLL.Models;
using RealEstate.DAL.Entities;
using RealEstate.Domain.Models;
using RealEstate.Domain.QueryParameters;

namespace RealEstate.BLL.Interfaces
{
    public interface IRealEstateService : IGenericService<RealEstateEntity, RealEstateModel>
    {
        Task<PagedEntityModel<RealEstateModel>> GetAllWithRequestParameters(RealEstateFilterParameters filters, CancellationToken ct);
    }
}
