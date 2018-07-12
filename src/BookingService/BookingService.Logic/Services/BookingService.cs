using System;
using System.Threading.Tasks;
using BookingService.CustomerServiceApi.Interfaces;
using BookingService.Domain;
using BookingService.Infrastructure.Repositories;
using BookingService.Logic.Interfaces;
using BookingService.MessageQueue;
using BookingService.MessageQueue.Events;
using CorrelationId;
using Mapster;

namespace BookingService.Logic.Services
{
    public class BookingService : IBookingService
    {
        private readonly BookingRepository _bookingRepository;
        private readonly RabbitMqClient _eventBus;
        private readonly ICustomerServiceApiClient _customerServiceApiClient;
        private readonly ICorrelationContextAccessor _correlationContext;
        
        public BookingService(
            BookingRepository bookingRepository,
            RabbitMqClient eventBus,
            ICustomerServiceApiClient customerServiceApiClient, 
            ICorrelationContextAccessor correlationContext)
        {
            _bookingRepository = bookingRepository;
            _eventBus = eventBus;
            _customerServiceApiClient = customerServiceApiClient;
            _correlationContext = correlationContext;
        }
        
        public async Task<Booking> GetAsync(long id)
        {
            var booking = await _bookingRepository.GetAsync(id);
            if (booking == null)
                throw new Exception($"Booking with id {id} does not exist");

            return booking;
        }

        public async Task<Booking> CreateAsync(string startTime, string endTime, long customerId)
        {
            var customer = await _customerServiceApiClient.GetCustomerByIdAsync(customerId, _correlationContext.CorrelationContext.CorrelationId);

            var booking = new Booking(DateTime.UtcNow, DateTime.UtcNow.AddDays(1), customerId, customer.FirstName);
            
            await _bookingRepository.CreateAsync(booking);
            await _bookingRepository.SaveChangesAsync();

            var @event = booking.Adapt<CreatedBookingEvent>();
            _eventBus.Publish(@event);
            
            return booking;
        }

        public async Task<Booking> UpdateAsync(long id, string startTime, string endTime)
        {
            var existedBooking = await _bookingRepository.GetAsync(id);
            if (existedBooking == null)
                throw new Exception($"Booking with id {id} does not exist");

            existedBooking.Update(DateTime.Parse(startTime), DateTime.Parse(endTime));
            
            _bookingRepository.Update(existedBooking);
            await _bookingRepository.SaveChangesAsync();

            return existedBooking;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var existedBooking = await _bookingRepository.GetAsync(id);
            if (existedBooking == null)
                throw new Exception($"Booking with id {id} does not exist");

            _bookingRepository.Delete(existedBooking);
            await _bookingRepository.SaveChangesAsync();
            
            return await _bookingRepository.SaveChangesAsync();
        }
    }
}