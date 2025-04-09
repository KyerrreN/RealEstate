using RealEstate.BLL.Models;
using RealEstate.DAL.Entities;
using RealEstate.DAL.Models;
using RealEstate.DAL.RequestParameters;

namespace RealEstate.BLL.Interfaces
{
    public interface IReviewService : IGenericService<ReviewEntity, ReviewModel>
    {
        Task<PagedEntityModel<ReviewModel>> GetReviewsOfUserAsync(PagingParameters paging, Guid userId, CancellationToken ct);
    }
}
