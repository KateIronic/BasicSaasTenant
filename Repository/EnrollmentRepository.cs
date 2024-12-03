using BasicSaasTenent.Models;
using Microsoft.EntityFrameworkCore;

namespace BasicSaasTenent.Repository
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Add a new enrollment user to an enrollment.
        /// </summary>
        public async Task<EnrollmentUser> AddEnrollmentUserAsync(EnrollmentUser enrollmentUser)
        {
            _context.EnrollmentUsers.Add(enrollmentUser);
            await _context.SaveChangesAsync();
            return enrollmentUser;
        }

        /// <summary>
        /// Retrieve or create an enrollment for a course.
        /// </summary>
        public async Task<Enrollment> GetOrCreateEnrollmentAsync(int courseId)
        {
            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.CourseId == courseId);

            if (enrollment == null)
            {
                enrollment = new Enrollment
                {
                    CourseId = courseId,
                    EnrolledAt = DateTime.UtcNow,
                    IsActive = true
                };
                _context.Enrollments.Add(enrollment);
                await _context.SaveChangesAsync();
            }

            return enrollment;
        }

        /// <summary>
        /// Get an enrollment user by user ID and course ID.
        /// </summary>
        public async Task<EnrollmentUser?> GetEnrollmentUserAsync(string userId, int courseId)
        {
            return await _context.EnrollmentUsers
                .Include(eu => eu.Enrollment)
                .FirstOrDefaultAsync(eu => eu.UserId == userId && eu.Enrollment.CourseId == courseId);
        }

        /// <summary>
        /// Get all enrollment users for a specific user.
        /// </summary>
        public async Task<IEnumerable<EnrollmentUser>> GetEnrollmentUsersByUserIdAsync(string userId)
        {
            return await _context.EnrollmentUsers
                .Include(eu => eu.Enrollment)
                .ThenInclude(e => e.Course)
                .Where(eu => eu.UserId == userId)
                .ToListAsync();
        }

        /// <summary>
        /// Get all enrollment users for a specific course.
        /// </summary>
        public async Task<IEnumerable<EnrollmentUser>> GetEnrollmentUsersByCourseIdAsync(int courseId)
        {
            return await _context.EnrollmentUsers
                .Include(eu => eu.User)
                .Where(eu => eu.Enrollment.CourseId == courseId)
                .ToListAsync();
        }

        /// <summary>
        /// Remove an enrollment user from an enrollment.
        /// </summary>
        public async Task<bool> RemoveEnrollmentUserAsync(string userId, int courseId)
        {
            var enrollmentUser = await GetEnrollmentUserAsync(userId, courseId);
            if (enrollmentUser == null)
                return false;

            _context.EnrollmentUsers.Remove(enrollmentUser);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Get a course by ID.
        /// </summary>
        public async Task<Course?> GetCourseAsync(int courseId)
        {
            return await _context.Courses.FindAsync(courseId);
        }
    }
}
