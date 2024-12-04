using BasicSaasTenent.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BasicSaasTenent.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<EnrollmentUser> EnrollmentUsers { get; set; }
        public DbSet<LoginModel> LoginModel { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Tenant Relationships
            modelBuilder.Entity<Tenant>()
                .HasMany(t => t.Users)
                .WithOne(u => u.Tenant)
                .HasForeignKey(u => u.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Course Relationships
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Tenant)
                .WithMany(t => t.Courses)
                .HasForeignKey(c => c.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Course>()
                .HasOne(c => c.Educator)
                .WithMany(e => e.CoursesCreated)
                .HasForeignKey(c => c.EducatorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure EnrollmentUser Relationships
            modelBuilder.Entity<EnrollmentUser>()
                .HasOne(eu => eu.User)
                .WithMany(u => u.EnrollmentUsers)
                .HasForeignKey(eu => eu.UserId);

            modelBuilder.Entity<EnrollmentUser>()
                .HasOne(eu => eu.Enrollment)
                .WithMany(e => e.EnrollmentUsers)
                .HasForeignKey(eu => eu.EnrollmentId);

            // Configure Enrollment Relationships
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
