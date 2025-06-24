using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace Project.Models;

public partial class StudentManagementContext : DbContext
{
    public StudentManagementContext()
    {
    }

    public StudentManagementContext(DbContextOptions<StudentManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Enrollment> Enrollments { get; set; }

    public virtual DbSet<Instructor> Instructors { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = new ConfigurationBuilder()
.SetBasePath(Directory.GetCurrentDirectory())
.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        IConfigurationRoot configuration = builder.Build();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("MyCnn"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Courses__C92D71A7330521DB");

            entity.HasIndex(e => e.Code, "IX_Courses_Code");

            entity.HasIndex(e => e.Code, "UQ__Courses__A25C5AA7DC83FDAF").IsUnique();

            entity.Property(e => e.Code).HasMaxLength(20);
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Instructor).WithMany(p => p.Courses)
                .HasForeignKey(d => d.InstructorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Course_Instructor");
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(e => e.EnrollmentId).HasName("PK__Enrollme__7F68771BA5B5980C");

            entity.HasIndex(e => e.CourseId, "IX_Enrollments_CourseId");

            entity.HasIndex(e => e.StudentId, "IX_Enrollments_StudentId");

            entity.Property(e => e.EnrollmentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Grade).HasColumnType("decimal(3, 2)");

            entity.HasOne(d => d.Course).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Enrollment_Course");

            entity.HasOne(d => d.Student).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Enrollment_Student");
        });

        modelBuilder.Entity<Instructor>(entity =>
        {
            entity.HasKey(e => e.InstructorId).HasName("PK__Instruct__9D010A9B19628FDB");

            entity.HasIndex(e => e.EmployeeCode, "UQ__Instruct__1F642548AA03CCB8").IsUnique();

            entity.Property(e => e.EmployeeCode).HasMaxLength(20);

            entity.HasOne(d => d.User).WithMany(p => p.Instructors)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Instructor_User");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Students__32C52B999EE502A1");

            entity.HasIndex(e => e.StudentCode, "IX_Students_StudentCode");

            entity.HasIndex(e => e.StudentCode, "UQ__Students__1FC88604401C52CB").IsUnique();

            entity.Property(e => e.StudentCode).HasMaxLength(20);

            entity.HasOne(d => d.User).WithMany(p => p.Students)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Student_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CC6429706");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E4C7C2EBC7").IsUnique();

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Role).HasMaxLength(20);
            entity.Property(e => e.Username).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
