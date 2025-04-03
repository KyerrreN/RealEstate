using RealEstate.DAL.Interfaces;
using RealEstate.DAL.Entities;

namespace RealEstate.DAL.Repositories
{
    public class HistoryRepository : BaseRepository<HistoryEntity>, IHistoryRepository
    {
        public HistoryRepository(AppDbContext context) : base(context) { }
    }
}
