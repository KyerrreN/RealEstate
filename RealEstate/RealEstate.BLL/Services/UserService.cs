using MassTransit;
using RealEstate.BLL.Interfaces;
using RealEstate.BLL.Models;
using RealEstate.DAL.Entities;
using RealEstate.DAL.Interfaces;
using NotificationService.Contracts;
using NotificationService.Contracts.Constants;
using Mapster;
using RealEstate.Domain.Exceptions;

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

            var userEvent = createdUser.Adapt<UserRegisteredEvent>();

            await publishEndpoint.Publish(userEvent, publishContext =>
            {
                publishContext.SetRoutingKey(NotificationConstants.UserRoutingKey);
            }, ct);

            return createdUser;
        }

        public async Task<UserModel> GetByAuth0IdAsync(string auth0Id, CancellationToken ct)
        {
            var userEntity = await _repository.FindOneByConditionAsync(u => u.Auth0Id == auth0Id, ct)
                ?? throw new NotFoundException(auth0Id);

            var userModel = userEntity.Adapt<UserModel>();

            return userModel;
        }
    }
}
