using RestaurantApp.Data;
using RestaurantApp.Models;
using RestaurantApp.Repository.IRepository;

namespace RestaurantApp.Repository
{
    public class AuthRepository : Repository<Auth>, IAuthRepository
    {
        private readonly DataContextEf _ef;

        public AuthRepository(DataContextEf ef) : base(ef)
        {
            _ef = ef;
        }

        public Auth UserExists(string email)
        {
            return _ef.Auths.FirstOrDefault(u => u.Email == email);
        }

        public int SelectUserId(string email)
        {
            return _ef.Users.Where(u => u.Email == email).Select(u => u.UserId).FirstOrDefault();
        }

    }
}
