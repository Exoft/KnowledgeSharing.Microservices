namespace CustomerService.Domain
{
    public class Customer
    {
        public long Id { get; set; }
        
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        public Customer(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public void Update(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}