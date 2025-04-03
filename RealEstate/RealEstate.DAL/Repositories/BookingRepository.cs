using RealEstate.DAL.Entities;
using RealEstate.DAL.Interfaces;

namespace RealEstate.DAL.Repositories
{
    public class BookingRepository : BaseRepository<BookingEntity>, IBookingRepository
    {
        public BookingRepository(AppDbContext context) : base(context) { }
    }
}
