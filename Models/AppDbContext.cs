using Microsoft.EntityFrameworkCore;

namespace StudentScoreManagement.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<Sponsor> Sponsors { get; set; }
        public DbSet<ProjectFunding> ProjectFundings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Many-to-many composite primary key for ProjectMembers
            modelBuilder.Entity<ProjectMember>()
                .HasKey(pm => new { pm.ProjectID, pm.StudentID });

            // Configure relations to prevent multiple cascade paths in MS SQL Server
            modelBuilder.Entity<Instructor>()
                .HasOne(i => i.Manager)
                .WithMany(i => i.Subordinates)
                .HasForeignKey(i => i.ManagerID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Course>()
                .HasOne(c => c.Department)
                .WithMany(d => d.Courses)
                .HasForeignKey(c => c.DepartmentID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Course>()
                .HasOne(c => c.Instructor)
                .WithMany(i => i.Courses)
                .HasForeignKey(c => c.InstructorID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Department)
                .WithMany(d => d.Students)
                .HasForeignKey(s => s.DepartmentID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Project>()
                .HasOne(p => p.LeadInstructor)
                .WithMany(i => i.ProjectsLed)
                .HasForeignKey(p => p.LeadInstructorID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProjectTask>()
                .HasOne(t => t.AssignedStudent)
                .WithMany(s => s.ProjectTasks)
                .HasForeignKey(t => t.AssignedStudentID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
