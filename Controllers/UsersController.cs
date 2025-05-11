using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using edusync.Models;
using Microsoft.Extensions.Configuration; // Add this
using System.IdentityModel.Tokens.Jwt;

namespace edusync.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly EduSyncDbContext _context;
        private readonly IConfiguration _configuration; // Add this

        // Inject IConfiguration in the constructor
        public UsersController(EduSyncDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration; // Initialize _configuration
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }

        public class LoginDto
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] User user)
        {
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            {
                return Conflict(new { message = "Email is already registered." });
            }

            user.UserId = Guid.NewGuid();
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Generate JWT token after user is created
            var token = JwtHelper.GenerateJwtToken(user.UserId.ToString(), user.Name, user.Role, _configuration); // Use _configuration here

            return Ok(new
            {
                token,
                user = new
                {
                    id = user.UserId,
                    name = user.Name,
                    email = user.Email,
                    role = user.Role
                }
            });
        }

        // POST: api/Users/login
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login([FromBody] LoginDto loginDto)
        {
            // Validate user credentials
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email && u.PasswordHash == loginDto.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            // Generate the JWT token after validating the credentials
            var token = JwtHelper.GenerateJwtToken(user.UserId.ToString(), user.Name, user.Role, _configuration);

            // Return the token and user info in the response
            return Ok(new
            {
                token,
                user = new
                {
                    id = user.UserId,
                    name = user.Name,
                    email = user.Email,
                    role = user.Role
                }
            });
        }
    }
}
