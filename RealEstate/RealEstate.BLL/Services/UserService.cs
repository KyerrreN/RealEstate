using MapsterMapper;
using RealEstate.BLL.Interfaces;
using RealEstate.BLL.Models;
using RealEstate.DAL.Entities;
using RealEstate.DAL.Interfaces;

namespace RealEstate.BLL.Services
{
    public class UserService(IBaseRepository<UserEntity> repository, IMapper mapper) 
        : GenericService<UserEntity, UserModel>(repository, mapper), IUserService
    {

    }
}
