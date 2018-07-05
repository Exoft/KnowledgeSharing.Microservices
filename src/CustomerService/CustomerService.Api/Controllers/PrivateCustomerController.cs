using System.Threading.Tasks;
using CorrelationId;
using CustomerService.Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.Api.Controllers
{
    [AllowAnonymous]
    [Route("papi/Customers")]
    public class PrivateCustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly ICorrelationContextAccessor _correlationContext;
        
        public PrivateCustomerController(
            ICustomerService customerService,
            ICorrelationContextAccessor correlationContext)
        {
            _customerService = customerService;
            _correlationContext = correlationContext;
        }
        
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] long id)
        {
            var customer = await _customerService.GetAsync(id);
            return Json(new {data = customer, isSuccess = true});
        }
    }
}