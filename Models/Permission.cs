namespace BasicSaasTenent.Models
{
    public class Permission
    {
        public int PermissionId { get; set; }  // Primary Key
        public string Name { get; set; }  // Permission name (e.g., "CanCreateCourse", "CanViewLesson")
        public string Description { get; set; }  // Description of the permission

        // Navigation Property
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
