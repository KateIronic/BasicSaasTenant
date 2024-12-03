using System.ComponentModel.DataAnnotations;

namespace BasicSaasTenent.Models
{
    public class Lesson
    {
        [Key]
        public int LessonId { get; set; }  // Primary Key

        [Required]
        public int CourseId { get; set; }  // Foreign Key to Course

        [Required, MaxLength(200)]
        public string Title { get; set; }  // Lesson title

        [Required]
        public string Content { get; set; }  // Lesson content (e.g., video link, text, etc.)

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public Course Course { get; set; }
    }
}
