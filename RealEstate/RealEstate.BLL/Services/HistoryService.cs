using MapsterMapper;
using RealEstate.BLL.Interfaces;
using RealEstate.BLL.Models;
using RealEstate.DAL.Entities;
using RealEstate.DAL.Interfaces;

namespace RealEstate.BLL.Services
{
    public class HistoryService(IBaseRepository<HistoryEntity> repository, IMapper mapper) : GenericService<HistoryEntity, HistoryModel>(repository, mapper), IHistoryService
    {
    }
}
