using System.Threading.Tasks;
using CustomerService.Api.Models.RequestModels;
using CustomerService.Api.Models.ResponseModels;
using CustomerService.Logic.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.Api.Controllers
{
    [AllowAnonymous]
    [Route("api/Customers")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] long id)
        {
            var customer = await _customerService.GetAsync(id);
            return Json(customer);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateCustomerRequestModel model)
        {
            var createdCustomer = await _customerService.CreateAsync(model.FirstName, model.LastName);

            var response = createdCustomer.Adapt<CustomerResponseModel>();
            return Json(response);
        }
        
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateCustomerRequestModel model)
        {
            var updatedCustomer = await _customerService.UpdateAsync(model.Id, model.FirstName, model.LastName);

            var response = updatedCustomer.Adapt<CustomerResponseModel>();
            return Json(response);
        }
        
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
        {
            var response = await _customerService.DeleteAsync(id);
            return Json(response);
        }
    }
}