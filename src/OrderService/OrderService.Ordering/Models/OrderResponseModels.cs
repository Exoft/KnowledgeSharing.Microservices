namespace OrderService.Ordering.Models
{
    public class OrderResponseModel
    {
        public long Id { get; set; }
        
        public string Description { get; private set; }
        public long CustomerId { get; private set; }
        public string NameOfCustomer { get; private set; }
    }
}