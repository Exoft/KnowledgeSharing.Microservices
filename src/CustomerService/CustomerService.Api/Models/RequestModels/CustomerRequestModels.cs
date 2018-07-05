namespace CustomerService.Api.Models.RequestModels
{
    public class BaseCustomerRequstModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
    
    public class CreateCustomerRequestModel : BaseCustomerRequstModel
    {
    }
    
    public class UpdateCustomerRequestModel : BaseCustomerRequstModel
    {
        public long Id { get; set; }
    }
}