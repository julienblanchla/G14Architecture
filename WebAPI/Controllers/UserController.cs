using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly G14DB _context;

        public UserController(G14DB context)
        {
            _context = context;
        }

        // GET: api/user
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var usersWithCards = await _context.User.Include(u => u.Card).ToListAsync();
            return Ok(usersWithCards);
        }

        // GET: api/user/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.User
                                     .Include(u => u.Card)  // Include the Card entity
                                     .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/user
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // PUT: api/user/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
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

        // DELETE: api/user/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/user/login
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login([FromBody] LoginModel loginModel)
        {
            var user = await _context.User
                .FirstOrDefaultAsync(u => u.Username == loginModel.Username && u.Password == loginModel.Password);

            if (user == null)
            {
                return Unauthorized(new { Message = "Invalid username or password" });
            }

            return Ok(user);
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.UserId == id);
        }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
