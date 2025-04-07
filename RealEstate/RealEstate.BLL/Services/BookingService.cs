using MapsterMapper;
using RealEstate.BLL.Interfaces;
using RealEstate.BLL.Models;
using RealEstate.DAL.Entities;
using RealEstate.DAL.Interfaces;

namespace RealEstate.BLL.Services
{
    public class BookingService(IBaseRepository<BookingEntity> repository, IMapper mapper) : GenericService<BookingEntity, BookingModel>(repository, mapper), IBookingService
    {
    }
}
