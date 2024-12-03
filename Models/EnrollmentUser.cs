using System.ComponentModel.DataAnnotations;

namespace BasicSaasTenent.Models
{
    public class EnrollmentUser
    {
        [Key]
        public int EnrollmentUserId { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        [Required]
        public int EnrollmentId { get; set; }

        public Enrollment Enrollment { get; set; }

        [Required]
        public UserRole Role { get; set; } // Student or Teacher

        public bool IsActive { get; set; } = true;

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}
