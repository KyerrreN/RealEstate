using RealEstate.DAL.Entities;
using RealEstate.DAL.Interfaces;

namespace RealEstate.DAL.Repositories
{
    public class RealEstateRepository : BaseRepository<RealEstateEntity>, IRealEstateRepository
    {
        public RealEstateRepository(AppDbContext context) : base(context) { }
    }
}
