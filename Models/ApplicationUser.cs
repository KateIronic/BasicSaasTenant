using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BasicSaasTenent.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string TenantId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Tenant Tenant { get; set; }

        public ICollection<Course>? CoursesCreated { get; set; }
        public ICollection<EnrollmentUser>? EnrollmentUsers { get; set; } = new List<EnrollmentUser>();
    }
}
