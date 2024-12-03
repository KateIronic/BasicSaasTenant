using BasicSaasTenent.Services;
using BasicSaasTenent.Services.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BasicSaasTenent.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class EnrollmentController(EnrollmentService service) : ControllerBase
    {
        private readonly EnrollmentService _service = service;

        /// <summary>
        /// Enroll a user in a course with a specified role.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> EnrollUser([FromBody] EnrollmentDTO enrollmentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var enrollmentUser = await _service.EnrollUserAsync(
                enrollmentDto.UserId,
                enrollmentDto.CourseId,
                enrollmentDto.Role);

            if (enrollmentUser == null)
                return BadRequest("Enrollment failed. Ensure the course and user exist and are active.");

            return CreatedAtAction(nameof(GetEnrollmentsByUser), new { userId = enrollmentDto.UserId }, enrollmentUser);
        }

        /// <summary>
        /// Get all enrollments for a specific user.
        /// </summary>
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetEnrollmentsByUser(string userId)
        {
            var enrollments = await _service.GetEnrollmentsByUserAsync(userId);

            if (enrollments == null || !enrollments.Any())
                return NotFound("No enrollments found for the specified user.");

            return Ok(enrollments);
        }

        /// <summary>
        /// Get all users enrolled in a specific course.
        /// </summary>
        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetUsersByCourse(int courseId)
        {
            var users = await _service.GetUsersByCourseAsync(courseId);

            if (users == null || !users.Any())
                return NotFound("No users found for the specified course.");

            return Ok(users);
        }

        /// <summary>
        /// Unenroll a user from a course.
        /// </summary>
        [HttpDelete("{userId}/{courseId}")]
        public async Task<IActionResult> UnenrollUser(string userId, int courseId)
        {
            var result = await _service.UnenrollUserAsync(userId, courseId);

            if (!result)
                return NotFound("Enrollment not found or already removed.");

            return NoContent();
        }
    }
}
