using Microsoft.EntityFrameworkCore;
using RestaurantApp.Data;
using RestaurantApp.Models;
using RestaurantApp.Repository;
using RestaurantApp.Repository.IRepository;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    private readonly DataContextEf _ef;

    public OrderRepository(DataContextEf ef) : base(ef)
    {
        _ef = ef;
    }

    public Order? GetByUserId(int userId)
    {
        return _ef.Orders.Include(o => o.Items).FirstOrDefault(o => o.CustomerId == userId);
    }
}