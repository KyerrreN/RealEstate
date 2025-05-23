using MassTransit;
using RealEstate.BLL.Interfaces;
using RealEstate.BLL.Models;
using RealEstate.DAL.Entities;
using RealEstate.DAL.Interfaces;
using NotificationService.Contracts;

namespace RealEstate.BLL.Services
{
    public class UserService(
        IBaseRepository<UserEntity> _repository,
        IPublishEndpoint publishEndpoint) 
        : GenericService<UserEntity, UserModel>(_repository), IUserService
    {
        public override async Task<UserModel> CreateAsync(UserModel model, CancellationToken ct)
        {
            var createdUser = await base.CreateAsync(model, ct);

            var userEvent = new UserRegisteredEvent
            {
                FirstName = createdUser.FirstName,
                LastName = createdUser.LastName,
                Email = createdUser.Email,
            };

            await publishEndpoint.Publish(userEvent, publishContext =>
            {
                publishContext.SetRoutingKey("user");
            }, ct);

            return createdUser;
        }
    }
}
