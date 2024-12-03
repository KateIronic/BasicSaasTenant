using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BasicSaasTenent.Models
{
    public class Enrollment
    {
        [Key]
        public int EnrollmentId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [JsonIgnore]
        public Course Course { get; set; }

        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        // Navigation Property for Many-to-Many Relationship with Users
        public ICollection<EnrollmentUser> EnrollmentUsers { get; set; } = new List<EnrollmentUser>();
    }
}
