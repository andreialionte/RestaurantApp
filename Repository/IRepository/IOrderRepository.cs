using RestaurantApp.Models;

namespace RestaurantApp.Repository.IRepository
{
    public interface IOrderRepository : IRepository<Order>
    {
        Order? GetByUserId(int userId);
    }
}
