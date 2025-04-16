using RealEstate.BLL.Models;
using RealEstate.DAL.Entities;
using RealEstate.Domain.Models;
using RealEstate.Domain.QueryParameters;

namespace RealEstate.BLL.Interfaces
{
    public interface IHistoryService : IGenericService<HistoryEntity, HistoryModel>
    {
        Task<PagedEntityModel<HistoryModel>> GetAllByOwnerIdAsync(PagingParameters paging, Guid ownerId, CancellationToken ct);
        Task<HistoryModel> GetOneByOwnerIdAsync(Guid historyId, Guid ownerId, CancellationToken ct);
        Task DeleteFromHistoryAsync(Guid historyId, Guid ownerId, CancellationToken ct);
    }
}
