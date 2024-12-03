namespace BasicSaasTenent.Models
{
    public class RolePermission
    {
        public int RolePermissionId { get; set; }
        public UserRole Role { get; set; }  // Educator or Student

        public int PermissionId { get; set; } // Foreign Key to Permission
        public Permission Permission { get; set; }  // Permission assigned to the role
    }

}