using RestaurantApp.Models;

namespace RestaurantApp.Repository.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        User GetByEmail(string email);
    }
}
