using BasicSaasTenent.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BasicSaasTenent.Controllers
{
    [Authorize]
    [Route("/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CourseController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet]
        public IActionResult GetAllCourses() {
            var TenantId = _httpContextAccessor.HttpContext?.Items["TenantId"] as string;
            var courses = _context.Courses.Where(c=>c.TenantId==TenantId).ToList();
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public IActionResult GetCourse(int id) {
            var TenantId = _httpContextAccessor.HttpContext?.Items["TenantId"] as string;

            var course = _context.Courses.FirstOrDefault(c=>c.TenantId==TenantId && c.CourseId==id);
            if (course == null) {
                return NotFound(); 
            }
            return Ok(course);
        }

        [Authorize(Roles = "Educator")]
        [HttpPost]
        public IActionResult CreateCourse([FromBody] Course course) {
            var TenantId = _httpContextAccessor.HttpContext?.Items["TenantId"] as string;

            course.TenantId = TenantId;
            _context.Courses.Add(course);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetCourse), new { id = course.CourseId }, course);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCourse([FromBody] Course course, int id) {
            var TenantId = _httpContextAccessor.HttpContext?.Items["TenantId"] as string;

            var existingCourse = _context.Courses.FirstOrDefault(c=>c.TenantId==TenantId && c.CourseId==id);
            if (existingCourse == null) { 
                return NotFound();
            }
            existingCourse.Title = course.Title;
            existingCourse.Description = course.Description;
            existingCourse.UpdatedAt = DateTime.UtcNow;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCourse(int id) {
            var TenantId = _httpContextAccessor.HttpContext?.Items["TenantId"] as string;

            var course = _context.Courses.FirstOrDefault(c=>c.TenantId==TenantId && c.CourseId==id);
            if (course == null) { 
                return NotFound();
            }
            _context.Courses.Remove(course);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
