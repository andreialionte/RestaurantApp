using RestaurantApp.Data;
using RestaurantApp.Models;
using RestaurantApp.Repository.IRepository;

namespace RestaurantApp.Repository
{
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        private readonly DataContextEf _ef;

        public PaymentRepository(DataContextEf ef) : base(ef)
        {
            _ef = ef;
        }


    }
}