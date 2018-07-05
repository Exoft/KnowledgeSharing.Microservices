using System;

namespace BookingService.Domain
{
    public class Booking
    {
        public long Id { get; set; }
        
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        
        public long CustomerId { get; private set; }
        public string NameOfCustomer { get; private set; }

        public Booking(DateTime startTime, DateTime endTime, long customerId, string nameOfCustomer)
        {
            if (startTime >= endTime)
                throw new Exception("Start time must be less end time");
            
            StartTime = startTime;
            EndTime = endTime;
            CustomerId = customerId;
            NameOfCustomer = nameOfCustomer;
        }

        public void Update(DateTime startTime, DateTime endTime)
        {
            if (startTime >= endTime)
                throw new Exception("Start time must be less end time");

            StartTime = startTime;
            EndTime = endTime;
        }
    }
}