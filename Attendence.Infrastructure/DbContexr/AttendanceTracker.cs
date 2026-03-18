using AttendenceTracker.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AttendenceTracker.Domain.Entity   
{
    public class AttendanceDbContext : DbContext
    {
        public AttendanceDbContext(DbContextOptions<AttendanceDbContext> options) : base(options)

        {


        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserDetails> UsersDetails { get; set; }
        public DbSet<Attendance> Attendances { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User → Role
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleID);

            // UserDetails → User
            modelBuilder.Entity<UserDetails>()
                .HasOne(d => d.User)
                .WithMany(u => u.UserDetails)
                .HasForeignKey(d => d.UserID);

            // ✅ Attendance → Student (FIX HERE)
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.User)
                .WithMany(u => u.Attendances)
                .HasForeignKey(a => a.UserId) // ✅ CORRECT
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ Attendance → RecordedBy (correct already)
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.RecordedUser)
                .WithMany(u => u.RecordedBy)
                .HasForeignKey(a => a.RecordedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}