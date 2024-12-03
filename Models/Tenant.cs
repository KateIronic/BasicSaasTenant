using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BasicSaasTenent.Models
{
    public class Tenant:ITenantGetter,ITenantSetter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }//subdomain for countries specific like in, jp, us
        public string Name { get; set; }
        public bool IsActive { get; set; }  // Status of the tenant (active/inactive)
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        // Navigation Properties
        public ICollection<ApplicationUser>? Users { get; set; }  // All users under the tenant
        public ICollection<Course>? Courses { get; set; }  // Courses offered by the tenant


    }
}
