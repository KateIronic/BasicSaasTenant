using BasicSaasTenent.Models;
using BasicSaasTenent.Repository;

namespace BasicSaasTenent.Services
{
    public interface IEnrollmentService
    {
        public Task<EnrollmentUser> EnrollUserAsync(string userId, int courseId, UserRole role);
        public Task<IEnumerable<EnrollmentUser>> GetEnrollmentsByUserAsync(string userId);
        public Task<IEnumerable<EnrollmentUser>> GetUsersByCourseAsync(int courseId);
        public Task<bool> UnenrollUserAsync(string userId, int courseId);
    }
}
