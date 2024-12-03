using BasicSaasTenent.Models;
using BasicSaasTenent.Repository;

namespace BasicSaasTenent.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly EnrollmentRepository _repository;

        public EnrollmentService(EnrollmentRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Enroll a user in a course with a specified role.
        /// </summary>
        public async Task<EnrollmentUser> EnrollUserAsync(string userId, int courseId, UserRole role)
        {
            // Check if enrollment already exists
            var existingEnrollmentUser = await _repository.GetEnrollmentUserAsync(userId, courseId);
            if (existingEnrollmentUser != null)
                throw new InvalidOperationException("User is already enrolled in this course.");

            // Ensure the course exists
            var course = await _repository.GetCourseAsync(courseId);
            if (course == null)
                throw new InvalidOperationException("The specified course does not exist.");

            // Create or retrieve the enrollment
            var enrollment = await _repository.GetOrCreateEnrollmentAsync(courseId);

            // Add the user to the enrollment
            var enrollmentUser = new EnrollmentUser
            {
                UserId = userId,
                EnrollmentId = enrollment.EnrollmentId,
                Role = role,
                IsActive = true,
                JoinedAt = DateTime.UtcNow
            };

            return await _repository.AddEnrollmentUserAsync(enrollmentUser);
        }

        /// <summary>
        /// Get all enrollments for a specific user.
        /// </summary>
        public async Task<IEnumerable<EnrollmentUser>> GetEnrollmentsByUserAsync(string userId)
        {
            return await _repository.GetEnrollmentUsersByUserIdAsync(userId);
        }

        /// <summary>
        /// Get all users enrolled in a specific course.
        /// </summary>
        public async Task<IEnumerable<EnrollmentUser>> GetUsersByCourseAsync(int courseId)
        {
            return await _repository.GetEnrollmentUsersByCourseIdAsync(courseId);
        }

        /// <summary>
        /// Unenroll a user from a course.
        /// </summary>
        public async Task<bool> UnenrollUserAsync(string userId, int courseId)
        {
            return await _repository.RemoveEnrollmentUserAsync(userId, courseId);
        }
    }
}
