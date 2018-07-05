using System.Threading.Tasks;
using BookingService.Api.Models.RequestModels;
using BookingService.Api.Models.ResponseModels;
using BookingService.Logic.Interfaces;
using CorrelationId;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Api.Controllers
{
    [AllowAnonymous]
    [Route("api/Bookings")]
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] long id)
        {
            var booking = await _bookingService.GetAsync(id);
            return Json(booking);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateBookingRequestModel model)
        {
            var createdBooking = await _bookingService.CreateAsync(model.StartTime, model.EndTime, model.CustomerId);

            var response = createdBooking.Adapt<BookingResponseModel>();
            return Json(response);
        }
        
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateBookingRequestModel model)
        {
            var updatedBooking = await _bookingService.UpdateAsync(model.Id, model.StartTime, model.EndTime);

            var response = updatedBooking.Adapt<BookingResponseModel>();
            return Json(response);
        }
        
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
        {
            var response = await _bookingService.DeleteAsync(id);
            return Json(response);
        }
    }
}