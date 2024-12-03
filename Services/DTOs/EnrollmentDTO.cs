using BasicSaasTenent.Models;
using System.ComponentModel.DataAnnotations;

namespace BasicSaasTenent.Services.DTOs
{
    public class EnrollmentDTO
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public UserRole Role { get; set; }

        public int EnrollmentId { get; set; }
        public CourseDto Course { get; set; }
    }
}
