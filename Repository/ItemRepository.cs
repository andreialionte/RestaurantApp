using RestaurantApp.Data;
using RestaurantApp.Models;
using RestaurantApp.Repository.IRepository;

namespace RestaurantApp.Repository
{
    public class ItemRepository : Repository<Item>, IItemRepository
    {
        private readonly DataContextEf _ef;

        public ItemRepository(DataContextEf ef) : base(ef)
        {
            _ef = ef;
        }
        public Item GetByName(string itemName)
        {
            return _ef.Items.FirstOrDefault(i => i.ItemName == itemName);
        }
    }
}
