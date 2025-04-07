using MapsterMapper;
using RealEstate.BLL.Interfaces;
using RealEstate.BLL.Models;
using RealEstate.DAL.Entities;
using RealEstate.DAL.Interfaces;

namespace RealEstate.BLL.Services
{
    public class RealEstateService(IBaseRepository<RealEstateEntity> repository, IMapper mapper) : GenericService<RealEstateEntity, RealEstateModel>(repository, mapper), IRealEstateService
    {
    }
}
