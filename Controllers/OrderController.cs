using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantApp.Data;
using RestaurantApp.DTOs;
using RestaurantApp.Models;
using RestaurantApp.Repository.IRepository;

namespace RestaurantApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly DataContextEf _ef;
        private readonly IOrderRepository _orderRepository;


        public OrderController(DataContextEf ef, IOrderRepository orderRepository)
        {
            _ef = ef;
            _orderRepository = orderRepository;
        }


        [HttpGet("GetOrders")]
        public IActionResult GetOrders()
        {
            IEnumerable<Order> orders = _orderRepository.GetAll();
            return Ok(orders);
        }

        [HttpGet("GetOrder/{userId}")]
        public IActionResult GetOrder(int userId)
        {
            Order? orderDb = _orderRepository.GetByUserId(userId);
            if (orderDb == null)
            {
                throw new Exception($"The user {userId} does not have any orders!");
            }
            return Ok(orderDb);
        }

        [HttpPost("PlaceOrder")]
        public IActionResult PlaceOrder(int userId, OrderDto orderDto)
        {
            User? user = _ef.Users.FirstOrDefault(u => u.UserId == userId);
            List<Item> selectedItems = _ef.Items.Where(i => orderDto.SelectedItemIds.Contains(i.ItemId)).ToList();
            decimal totalAmount = 0;

            foreach (Item item in selectedItems)
            {
                totalAmount += item.Price;
            }

            Order orderDb = new Order()
            {
                CustomerId = user.UserId,
                TotalAmount = orderDto.TotalAmount,
                Status = "Processing",
                OrderDate = DateTime.Now,
                Items = selectedItems,

            };

            _orderRepository.Add(orderDb);
            _orderRepository.Save();

            return Ok(orderDb);
        }

        [HttpPut("UpdateOrder/{orderId}")]
        public IActionResult UpdateOrder(int orderId, OrderDto orderUpdateDto)
        {
            Order? orderDb = _ef.Orders.Include(o => o.Items).FirstOrDefault(o => o.OrderId == orderId);
            if (orderDb == null)
            {
                throw new Exception($"Order with ID {orderId} not found");
            }

            if (orderUpdateDto.TotalAmount != null)
            {
                orderDb.TotalAmount = orderUpdateDto.TotalAmount;
            }
            if (orderUpdateDto.Status != null)
            {
                orderDb.Status = orderUpdateDto.Status;
            }


            _orderRepository.Update(orderDb);
            _orderRepository.Save();

            return Ok(orderDb);
        }

        [HttpDelete("DeleteOrder/{orderId}")]
        public IActionResult DeleteOrder(int orderId)
        {
            // Find the order to delete
            Order? orderDb = _orderRepository.Get(orderId);
            if (orderDb == null)
            {
                throw new Exception($"Order with ID {orderId} not found");
            }

            _orderRepository.Remove(orderDb);
            _orderRepository.Save();

            return Ok(orderDb);
        }
    }
}