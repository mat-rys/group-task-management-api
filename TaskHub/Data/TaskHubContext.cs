using Microsoft.EntityFrameworkCore;
using TaskHub.Entities;

namespace TaskHub.Data
{
    public class TaskHubContext : DbContext
    {
        public TaskHubContext(DbContextOptions<TaskHubContext> options)
            : base(options) {
        }
        public DbSet<UserProfile> UserProfiles { get; set; } =  default!;
        public DbSet<TaskTodo> TaskTodos { get; set; } = default!;
        public DbSet<TaskTodoDetail> TaskTodoDetails { get; set; } = default!;
        public DbSet<TaskComment> TaskComments { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserProfile>()
                .HasMany(d => d.Tasks)
                .WithMany(d => d.UserProfiles)
                .UsingEntity(j => j.ToTable("UserTask"));
                
            modelBuilder.Entity<UserProfile>()
                .HasMany(d => d.TaskComments)
                .WithOne(d => d.UserProfile)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskTodo>()
                .HasOne(d => d.TaskDetail)
                .WithOne(d => d.TaskTodo)
                .HasForeignKey<TaskTodo>(d => d.TaskDetailId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskTodo>()
                .HasMany(d => d.TaskComments)
                .WithOne(d => d.TaskTodo)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }

}
