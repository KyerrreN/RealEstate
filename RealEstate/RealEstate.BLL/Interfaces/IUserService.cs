using RealEstate.BLL.Models;
using RealEstate.DAL.Entities;

namespace RealEstate.BLL.Interfaces
{
    public interface IUserService : IGenericService<UserEntity, UserModel>
    {
        Task<UserModel> GetByAuth0IdAsync(string auth0Id, CancellationToken ct);
    }
}
