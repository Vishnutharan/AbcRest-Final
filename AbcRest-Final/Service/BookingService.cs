using System.Collections.Generic;
using System.Threading.Tasks;
using AbcRest_Final.Interface;
using AbcRest_Final.Model;

namespace AbcRest_Final.Service
{
    public class BookingService : IBookingService
    {
        private readonly IRepository<Booking> _bookingRepository;
        private readonly EmailService _emailService;

        public BookingService(IRepository<Booking> bookingRepository, EmailService emailService)
        {
            _bookingRepository = bookingRepository;
            _emailService = emailService;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _bookingRepository.GetAllAsync();
        }

        public async Task<Booking> GetBookingByIdAsync(int id)
        {
            return await _bookingRepository.GetByIdAsync(id);
        }

        public async Task AddBookingAsync(Booking booking)
        {
            await _bookingRepository.AddAsync(booking);

            // Send email notification after booking is added
            var emailSubject = "Booking Confirmation";
            var emailBody = $"Dear {booking.Name}, your booking for {booking.Persons} person(s) on {booking.Date.ToShortDateString()} at {booking.Time} has been confirmed!";
            await _emailService.SendBookingNotificationAsync(booking.Email, emailSubject, emailBody);
        }

        public async Task UpdateBookingAsync(Booking booking)
        {
            await _bookingRepository.UpdateAsync(booking);
        }

        public async Task DeleteBookingAsync(int id)
        {
            await _bookingRepository.DeleteAsync(id);
        }
    }
}
