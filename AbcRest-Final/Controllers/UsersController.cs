using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AbcRest_Final.Database_Context;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AbcRest_Final.Model;

namespace AbcRest_Final.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        // Constructor to inject the database context
        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET method to retrieve all users
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            // Check if the Users DBSet is null
            if (_context.Users == null)
            {
                return NotFound("No users found.");
            }

            // Retrieve users with null handling for optional fields
            var users = await _context.Users.Select(user => new
            {
                id = user.id,
                FirstName = user.FirstName ?? "Default FirstName", // Default value if null
                LastName = user.LastName ?? "Default LastName",    // Default value if null
                Email = user.Email ?? "Default Email",             // Default value if null
                UserName = user.UserName,
                Password = user.Password
            }).ToListAsync();

            return Ok(users);
        }

        // POST method for user login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            // Validate model state
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check user credentials
            var user = await _context.Users
                                     .FirstOrDefaultAsync(u => u.UserName == loginDto.UserName && u.Password == loginDto.Password);

            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            return Ok(new { Message = "Login successful.", UserId = user.id });
        }

        // POST method for user registration
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            // Validate model state
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the username already exists
            bool userExists = await _context.Users
                                            .AnyAsync(u => u.UserName == registerDto.UserName);

            if (userExists)
            {
                return Conflict("Username is already taken.");
            }

            var user = new User
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                Password = registerDto.Password,
                // Uncomment and use the following lines if FirstName and LastName are included in RegisterDto
                // FirstName = registerDto.FirstName,
                // LastName = registerDto.LastName
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.id }, user);
        }

        // GET method to retrieve a single user by ID
        [HttpGet("users/{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            return user;
        }
    }
}
