using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Ordering.Commands;
using OrderService.Ordering.Queries;

namespace OrderService.Api.Controllers
{
    [AllowAnonymous]
    [Route("api/Orders")]
    public class OrderController : Controller
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] long id)
        {
            var query = new GetOrderByIdQuery
            {
                Id = id
            };
            
            var response = await _mediator.Send(query);
            return Json(response);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateOrderCommand command)
        {   
            var response = await _mediator.Send(command);
            return Json(response);
        }
        
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateOrderCommand command)
        {   
            var response = await _mediator.Send(command);
            return Json(response);
        }
        
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
        {   
            var command = new DeleteOrderCommand
            {
                Id = id
            };
            
            var response = await _mediator.Send(command);
            return Json(response);
        }
    }
}