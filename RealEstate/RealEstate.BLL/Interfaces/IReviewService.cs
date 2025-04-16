using RealEstate.BLL.Models;
using RealEstate.DAL.Entities;
using RealEstate.Domain.Models;
using RealEstate.Domain.QueryParameters;

namespace RealEstate.BLL.Interfaces
{
    public interface IReviewService : IGenericService<ReviewEntity, ReviewModel>
    {
        Task<PagedEntityModel<ReviewModel>> GetReviewsOfUserAsync(PagingParameters paging, Guid userId, CancellationToken ct);
    }
}
