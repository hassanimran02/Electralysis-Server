using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pleromi.DAL.DatabaseContext;
using Pleromi.DAL.Entities;
using BCrypt.Net;

namespace Pleromi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ElectralysisDbContext _context;

        public AuthController(ElectralysisDbContext context)
        {
            _context = context;
        }

        // POST: api/Users/SignIn
        [HttpPost("SignIn")]
        public async Task<ActionResult<User>> SignIn([FromBody] SignInRequest request)
        {
            try
            {
                var user = await _context.Users
                    .Where(u => u.MobileNo == request.Mobile)
                    .FirstOrDefaultAsync();

                if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                {
                    return Unauthorized("Invalid credentials");
                }

                return Ok(user);
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.InnerException?.Message);
                return StatusCode(500, "See the inner exception for details.");
            }
        }

        // POST: api/Auth/SignUp
        [HttpPost("SignUp")]
        public async Task<ActionResult<User>> SignUp([FromBody] SignUpRequest request)
        {
            try
            {
                // Log request data for debugging
                Console.WriteLine($"Received SignUpRequest: {JsonConvert.SerializeObject(request)}");

                // Check if the user already exists
                var existingUser = await _context.Users
                    .Where(u => u.MobileNo == request.Mobile)
                    .FirstOrDefaultAsync();

                if (existingUser != null)
                {
                    return Conflict("User with this mobile number already exists.");
                }

                // Hash the password
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

                // Create new user
                var newUser = new User
                {
                    FullName = request.FullName,
                    MobileNo = request.Mobile,
                    Password = hashedPassword,
                    StatusID = 2,
                    OTPVerified = true
                    // Set other properties as necessary
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                return Ok(newUser);
            }
            catch (Exception ex)
            {
                // Log the detailed exception message
                Console.WriteLine(ex.InnerException?.Message);
                return StatusCode(500, "An error occurred while saving the entity changes. See the inner exception for details.");
            }
        }
    }
}
