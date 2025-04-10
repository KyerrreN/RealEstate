using Mapster;
using MapsterMapper;
using RealEstate.BLL.Exceptions;
using RealEstate.BLL.Interfaces;
using RealEstate.BLL.Models;
using RealEstate.DAL.Entities;
using RealEstate.DAL.Interfaces;

namespace RealEstate.BLL.Services
{
    public class BookingService(IBaseRepository<BookingEntity> repository, IMapper mapper, IRealEstateRepository realEstateRepository, IBookingRepository bookingRepository, IUserRepository userRepository) : GenericService<BookingEntity, BookingModel>(repository, mapper), IBookingService
    {
        private readonly IRealEstateRepository _realEstateRepository = realEstateRepository;
        private readonly IBookingRepository _bookingRepository = bookingRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public override async Task<BookingModel> CreateAsync(BookingModel model, CancellationToken ct)
        {
            if (model is null)
                throw new BadRequestException();

            _ = await _userRepository.FindByIdAsync(model.UserId, ct)
                ?? throw new NotFoundException(model.UserId);

            var realEstateEntity = await _realEstateRepository.FindByIdAsync(model.RealEstateId, ct)
                ?? throw new NotFoundException(model.RealEstateId);

            if (realEstateEntity.OwnerId != model.UserId)
                throw new BadRequestException("User does not own this property");

            var entity = model.Adapt<BookingEntity>();

            var createdEntity = await _bookingRepository.CreateAsync(entity, ct);

            var createdModel = createdEntity.Adapt<BookingModel>();

            return createdModel;
        }
    }
}
