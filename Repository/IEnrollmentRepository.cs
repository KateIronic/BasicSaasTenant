using BasicSaasTenent.Models;

namespace BasicSaasTenent.Repository
{
    public interface IEnrollmentRepository
    {
        public Task<EnrollmentUser> AddEnrollmentUserAsync(EnrollmentUser enrollmentUser);
        public Task<Enrollment> GetOrCreateEnrollmentAsync(int courseId);
        public Task<EnrollmentUser?> GetEnrollmentUserAsync(string userId, int courseId);

        public Task<IEnumerable<EnrollmentUser>> GetEnrollmentUsersByUserIdAsync(string userId);
        public Task<IEnumerable<EnrollmentUser>> GetEnrollmentUsersByCourseIdAsync(int courseId);
        public Task<bool> RemoveEnrollmentUserAsync(string userId, int courseId);

        public Task<Course?> GetCourseAsync(int courseId);

    }
}
