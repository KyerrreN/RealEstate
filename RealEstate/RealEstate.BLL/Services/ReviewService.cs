using MapsterMapper;
using RealEstate.BLL.Interfaces;
using RealEstate.BLL.Models;
using RealEstate.DAL.Entities;
using RealEstate.DAL.Interfaces;

namespace RealEstate.BLL.Services
{
    public class ReviewService(IBaseRepository<ReviewEntity> repository, IMapper mapper) : GenericService<ReviewEntity, ReviewModel>(repository, mapper), IReviewService
    {
    }
}
