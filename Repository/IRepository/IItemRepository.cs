using RestaurantApp.Models;

namespace RestaurantApp.Repository.IRepository
{
    public interface IItemRepository : IRepository<Item>
    {
        // Define additional methods specific to Item repository if needed
        Item GetByName(string itemName);
    }
}
