using RealEstate.BLL.Interfaces;
using RealEstate.BLL.Models;
using RealEstate.DAL.Entities;
using RealEstate.DAL.Interfaces;

namespace RealEstate.BLL.Services
{
    public class UserService(IBaseRepository<UserEntity> _repository) 
        : GenericService<UserEntity, UserModel>(_repository), IUserService
    {

    }
}
