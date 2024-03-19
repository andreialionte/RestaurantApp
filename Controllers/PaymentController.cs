using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Data;
using RestaurantApp.Models;
using RestaurantApp.Repository.IRepository;
using Stripe;

namespace RestaurantApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly DataContextEf _ef;
        private readonly IConfiguration _config;
        private readonly IPaymentRepository _paymentRepository;

        public PaymentController(IConfiguration configuration, DataContextEf ef, IPaymentRepository paymentRepository)
        {
            _config = configuration;
            _ef = ef;
            _paymentRepository = paymentRepository;
        }

        [HttpPost("InitiatePayment")]
        public IActionResult InitiatePayment(int orderId)
        {
            try
            {
                string stripeSecretKey = _config.GetSection("Stripe:SecretKey").Value;
                StripeConfiguration.ApiKey = stripeSecretKey;

                Order? orderDb = _ef.Orders.FirstOrDefault(o => o.OrderId == orderId);
                if (orderDb == null)
                {
                    return NotFound(new { ErrorMessage = $"Order with id {orderId} not found!" });
                }

                decimal totalAmount = orderDb.TotalAmount;

                if (totalAmount <= 0)
                {
                    return BadRequest(new { ErrorMessage = "Total amount must be greater than 0." });
                }

                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)(totalAmount * 100),
                    Currency = "EURO",
                    Description = $"Payment for Order {orderId}"
                };

                var service = new PaymentIntentService();
                var paymentIntent = service.Create(options);

                return Ok(new { ClientSecret = paymentIntent.ClientSecret });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ErrorMessage = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpPost("ConfirmPayment")]
        public IActionResult ConfirmPayment(int orderId)
        {
            try
            {
                Order? orderDb = _ef.Orders.FirstOrDefault(o => o.OrderId == orderId);
                if (orderDb == null)
                {
                    return NotFound(new { ErrorMessage = $"Order with id {orderId} not found!" });
                }

                Payment payment = new Payment()
                {
                    OrderId = orderId,
                    TotalAmount = orderDb.TotalAmount,
                    PaymentDate = DateTime.Now,
                    PaymentStatus = "Success"
                };

                _paymentRepository.Add(payment);
                _paymentRepository.Save();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ErrorMessage = $"An error occurred: {ex.Message}" });
            }
        }
    }
}
