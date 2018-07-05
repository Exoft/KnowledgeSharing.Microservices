using System;
using System.Threading.Tasks;
using CustomerService.Domain;
using CustomerService.Infrastructure.Repositories;
using CustomerService.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CustomerService.Logic.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly CustomerRepository _customerRepository;
        private readonly ILogger _logger;
        
        public CustomerService(
            CustomerRepository customerRepository,
            ILoggerFactory loggerFactory)
        {
            _customerRepository = customerRepository;
            _logger = loggerFactory.CreateLogger(typeof(CustomerService).Name);
        }
        
        public async Task<Customer> GetAsync(long id)
        {
            var customer = await _customerRepository.GetAsync(id);
            if (customer == null)
                throw new Exception($"Customer with id {id} does not exist");

            return customer;
        }

        public async Task<Customer> CreateAsync(string firstName, string lastName)
        {
            var customer = new Customer(firstName, lastName);
            
            await _customerRepository.CreateAsync(customer);
            await _customerRepository.SaveChangesAsync();
            
            return customer;
        }

        public async Task<Customer> UpdateAsync(long id, string firstName, string lastName)
        {
            var existedCustomer = await _customerRepository.GetAsync(id);
            if (existedCustomer == null)
                throw new Exception($"Customer with id {id} does not exist");

            existedCustomer.Update(firstName, lastName);
            
            _customerRepository.Update(existedCustomer);
            await _customerRepository.SaveChangesAsync();

            return existedCustomer;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var existedCustomer = await _customerRepository.GetAsync(id);
            if (existedCustomer == null)
                throw new Exception($"Customer with id {id} does not exist");

            _customerRepository.Delete(existedCustomer);
            
            return await _customerRepository.SaveChangesAsync();
        }
    }
}