using AbcRest_Final.Model;
using AbcRest_Final.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AbcRest_Final.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly EmailService _emailService;

        public BookingController(IBookingService bookingService, EmailService emailService)
        {
            _bookingService = bookingService;
            _emailService = emailService;
        }

        // GET: api/Booking
        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return Ok(bookings);
        }

        // GET: api/Booking/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            return booking == null ? NotFound() : Ok(booking);
        }

        // POST: api/Booking
        [HttpPost]
        public async Task<IActionResult> AddBooking([FromBody] Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _bookingService.AddBookingAsync(booking);

            // Send notification email after booking is successfully added
            var emailSubject = "Booking Confirmation";
            var emailBody = $"Dear {booking.Name},\n\n" +
                            $"Thank you for your booking! Here are your booking details:\n\n" +
                            $"Booking ID: {booking.Id}\n" +
                            $"Name: {booking.Name}\n" +
                            $"Number of Persons: {booking.Persons}\n" +
                            $"Date: {booking.Date.ToShortDateString()}\n" +
                            $"Time: {booking.Time}\n\n" +
                            $"We look forward to serving you!\n\n" +
                            $"Best regards,\n" +
                            $"Your Company Name";

            await _emailService.SendBookingNotificationAsync(booking.Email, emailSubject, emailBody);

            return CreatedAtAction(nameof(GetBookingById), new { id = booking.Id }, booking);
        }

        // PUT: api/Booking/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] Booking booking)
        {
            if (id != booking.Id)
            {
                return BadRequest("ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingBooking = await _bookingService.GetBookingByIdAsync(id);
            if (existingBooking == null)
            {
                return NotFound();
            }

            await _bookingService.UpdateBookingAsync(booking);
            return NoContent();
        }

        // DELETE: api/Booking/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            await _bookingService.DeleteBookingAsync(id);
            return NoContent();
        }
    }
}



//[Controller] < --> [Service] < --> [Interface] < --> [Model]


//Client Request
//       |
//   [Controller]  <----> Handles HTTP requests, interacts with the Service
//       |
//   [Service]  <----> Contains business logic, interacts with the Interface
//       |
//   [Interface]  <----> Abstracts data operations, interacts with the Model
//       |
//   [Model]  <----> Represents the data structure
//Client Request
//       |
//   [Controller]  <----> Handles HTTP requests, interacts with the Service
//       |
//   [Service]  <----> Contains business logic, interacts with the Interface
//       |
//   [Interface]  <----> Abstracts data operations, interacts with the Model
//       |
//   [Model]  <----> Represents the data structure
