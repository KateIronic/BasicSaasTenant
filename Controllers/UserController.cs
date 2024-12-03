//using BasicSaasTenent.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Linq;
//using System.Threading.Tasks;

//namespace BasicSaasTenent.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class UserController : ControllerBase
//    {
//        private readonly ApplicationDbContext _context;

//        public UserController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        // GET: api/User
//        [HttpGet]
//        public async Task<IActionResult> GetUsers()
//        {
//            var users = await _context.Users.Include(u => u.Tenant).ToListAsync();
//            return Ok(users);
//        }

//        // GET: api/User/{id}
//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetUser(string id)
//        {
//            var user = await _context.Users
//                .Include(u => u.Tenant)
//                .Include(u => u.CoursesCreated)
//                .Include(u => u.Enrollments)
//                .ThenInclude(e => e.Course)
//                .FirstOrDefaultAsync(u => u.Id == id);

//            if (user == null)
//            {
//                return NotFound($"User with ID {id} not found.");
//            }

//            return Ok(user);
//        }

//        // POST: api/User
//        [HttpPost]
//        public async Task<IActionResult> CreateUser([FromBody] ApplicationUser user)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            user.CreatedAt = DateTime.UtcNow;
//            user.UpdatedAt = DateTime.UtcNow;

//            await _context.Users.AddAsync(user);
//            await _context.SaveChangesAsync();

//            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
//        }

//        // PUT: api/User/{id}
//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateUser(string id, [FromBody] ApplicationUser updatedUser)
//        {
//            if (id != updatedUser.Id)
//            {
//                return BadRequest("User ID mismatch.");
//            }

//            var existingUser = await _context.Users.FindAsync(id);
//            if (existingUser == null)
//            {
//                return NotFound($"User with ID {id} not found.");
//            }

//            existingUser.UserName = updatedUser.UserName;
//            existingUser.Email = updatedUser.Email;
//            existingUser.TenantId = updatedUser.TenantId;
//            existingUser.Role = updatedUser.Role;
//            existingUser.UpdatedAt = DateTime.UtcNow;

//            _context.Users.Update(existingUser);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        // DELETE: api/User/{id}
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteUser(string id)
//        {
//            var user = await _context.Users.FindAsync(id);
//            if (user == null)
//            {
//                return NotFound($"User with ID {id} not found.");
//            }

//            _context.Users.Remove(user);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }
//    }
//}
