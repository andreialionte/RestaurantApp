using RestaurantApp.Models;

namespace RestaurantApp.Repository.IRepository
{
    public interface IAuthRepository : IRepository<Auth>
    {
        Auth UserExists(string email);
        int SelectUserId(string email);
    }
}
