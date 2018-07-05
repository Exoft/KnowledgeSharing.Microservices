using System;

namespace OrderService.Domain.AggregatesModels.OrderAggregate
{
    public class Order
    {
        public long Id { get; set; }
        
        public DateTime OrderTime { get; private set; }
        public string Description { get; private set; }
        public long CustomerId { get; private set; }
        public string NameOfCustomer { get; private set; }
        
        public Order(string description, long customerId, string nameOfCustomer)
        {
            OrderTime = DateTime.UtcNow;
            Description = description;
            CustomerId = customerId;
            NameOfCustomer = nameOfCustomer;
        }

        public void Update(string description)
        {
            Description = description;
        }
    }
}