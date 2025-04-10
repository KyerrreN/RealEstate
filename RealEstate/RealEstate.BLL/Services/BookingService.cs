using Mapster;
using MapsterMapper;
using RealEstate.BLL.Exceptions;
using RealEstate.BLL.Interfaces;
using RealEstate.BLL.Models;
using RealEstate.DAL.Entities;
using RealEstate.DAL.Interfaces;
using RealEstate.DAL.Repositories;

namespace RealEstate.BLL.Services
{
    public class BookingService(IBaseRepository<BookingEntity> repository, IMapper mapper, IRealEstateRepository realEstateRepository, IBookingRepository bookingRepository, IUserRepository userRepository, AppDbContext context, IHistoryRepository historyRepository) : GenericService<BookingEntity, BookingModel>(repository, mapper), IBookingService
    {
        private readonly IRealEstateRepository _realEstateRepository = realEstateRepository;
        private readonly IBookingRepository _bookingRepository = bookingRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly AppDbContext _context = context;
        private readonly IHistoryRepository _historyRepository = historyRepository;

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

        public async Task CloseDeal(CloseDealModel model, CancellationToken ct)
        {
            var bookingEntity = await _bookingRepository.FindByIdAsync(model.Id, ct)
                ?? throw new NotFoundException(model.Id);

            var realEstateEntities = await _realEstateRepository.FindByConditionAsync(re => re.Bookings.First().Id == model.Id, ct);

            if (realEstateEntities.Count != 1)
            {
                throw new BadRequestException("Real Estate associated with this booking does not exist");
            }

            var historyModel = new HistoryModel
            {
                RealEstateId = realEstateEntities[0].Id,
                UserId = bookingEntity.UserId,
                CompletedAt = DateTime.UtcNow,
                EstateAction = model.EstateAction,
                Title = realEstateEntities[0].Title,
                Description = realEstateEntities[0].Description
            };

            var historyEntity = historyModel.Adapt<HistoryEntity>();

            using (var transaction = await _context.Database.BeginTransactionAsync(ct))
            {
                try
                {
                    await _bookingRepository.DeleteAsync(bookingEntity, ct);
                    await _historyRepository.CreateAsync(historyEntity, ct);
                    await _realEstateRepository.DeleteAsync(realEstateEntities[0], ct);

                    await transaction.CommitAsync(ct);
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(ct);
                    throw;
                }
            }
        }
    }
}
