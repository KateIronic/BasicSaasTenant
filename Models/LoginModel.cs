using System.ComponentModel.DataAnnotations;

namespace BasicSaasTenent.Models
{
    public class LoginModel
    {
        [Key]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        //[Required]
        //public string TenantId { get; set; }
    }

}
