using RealEstate.DAL.Entities;
using RealEstate.DAL.Interfaces;

namespace RealEstate.DAL.Repositories
{
    public class UserRepository :  BaseRepository<UserEntity>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }
    }
}
