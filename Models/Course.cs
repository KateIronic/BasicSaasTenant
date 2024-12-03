using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BasicSaasTenent.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }  // Primary Key

        [Required]
        public string TenantId { get; set; }  // Foreign Key to Tenant

        [Required]
        public string EducatorId { get; set; }  // Foreign Key to Educator (ApplicationUser)

        [Required, MaxLength(200)]
        public string Title { get; set; }  // Course title

        public string? Description { get; set; }  // Course description

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public ApplicationUser? Educator { get; set; }  // Educator who created the course
        public Tenant Tenant { get; set; }  // Tenant offering the course
        public ICollection<Lesson>? Lessons { get; set; }  // Lessons in the course

        [JsonIgnore]
        public ICollection<Enrollment>? Enrollments { get; set; } = new List<Enrollment>();
    }
}
