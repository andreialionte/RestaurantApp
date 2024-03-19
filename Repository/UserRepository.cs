using RestaurantApp.Data;
using RestaurantApp.Models;
using RestaurantApp.Repository.IRepository;

namespace RestaurantApp.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly DataContextEf _ef;

        public UserRepository(DataContextEf ef) : base(ef)
        {
            _ef = ef;
        }

        public User GetByEmail(string email)
        {
            return _ef.Users.FirstOrDefault(x => x.Email == email);
        }
    }
}
