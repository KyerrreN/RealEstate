using RealEstate.BLL.Models;
using RealEstate.DAL.Entities;
using RealEstate.DAL.Models;
using RealEstate.DAL.RequestParameters;

namespace RealEstate.BLL.Interfaces
{
    public interface IRealEstateService : IGenericService<RealEstateEntity, RealEstateModel>
    {
        Task<PagedEntityModel<RealEstateModel>> GetAllWithRequestParameters(RealEstateFilterParameters filters, CancellationToken ct);
    }
}
