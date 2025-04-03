using RealEstate.DAL.Entities;
using RealEstate.DAL.Interfaces;

namespace RealEstate.DAL.Repositories
{
    public class ReviewRepository : BaseRepository<ReviewEntity>, IReviewRepository
    {
        public ReviewRepository(AppDbContext context) : base(context) { }
    }
}
