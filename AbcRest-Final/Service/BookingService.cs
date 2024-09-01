using AbcRest_Final.Interface;
using AbcRest_Final.Model;
using AbcRest_Final.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AbcRest_Final.Service
{
    public class BookingService : IBookingService
    {
        private readonly IRepository<Booking> _bookingRepository;

        public BookingService(IRepository<Booking> bookingRepository)
        {
            _bookingRepository = bookingRepository;
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
