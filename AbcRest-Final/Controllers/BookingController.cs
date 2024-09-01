using AbcRest_Final.Model;
using AbcRest_Final.Service;
using Microsoft.AspNetCore.Mvc;



namespace AbcRest_Final.Controllers
{
    [ApiController]  // Explicit declaration as an API Controller
    [Route("api/[controller]")]  // Routing path for API
    public class BookingController : ControllerBase  // Use ControllerBase for API controllers
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // GET: api/Booking
        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return Ok(bookings);  // Return all bookings as JSON
        }

        // GET: api/Booking/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            return booking == null ? NotFound() : Ok(booking);  // Return a single booking or Not Found
        }

        // POST: api/Booking
        [HttpPost]
        public async Task<IActionResult> AddBooking([FromBody] Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  // Return bad request if model state is invalid
            }

            await _bookingService.AddBookingAsync(booking);
            return CreatedAtAction(nameof(GetBookingById), new { id = booking.Id }, booking);  // Return the created booking
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

            // Update the booking
            await _bookingService.UpdateBookingAsync(booking);
            return NoContent();  // Return no content after a successful update
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
            return NoContent();  // Return no content after a successful deletion
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
