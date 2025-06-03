using Microsoft.EntityFrameworkCore;
using System.Data;
using _1.Models;

namespace _1.Data
{
    public class IGDBContext : DbContext
    {
        public IGDBContext(DbContextOptions<IGDBContext> options) : base(options)
        {
        }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Audition> Auditions { get; set; }
        public DbSet<AuditionRequest> AuditionRequests { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<ScheduleRole> ScheduleRoles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка связи многие ко многим для UserRole
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<ScheduleRole>()
         .HasKey(sr => new { sr.ScheduleId, sr.RoleId });

            modelBuilder.Entity<ScheduleRole>()
                .HasOne(sr => sr.Schedule)
                .WithMany(s => s.ScheduleRoles)
                .HasForeignKey(sr => sr.ScheduleId);

            modelBuilder.Entity<ScheduleRole>()
                .HasOne(sr => sr.Role)
                .WithMany(r => r.ScheduleRoles)
                .HasForeignKey(sr => sr.RoleId);
        }
    }
}

