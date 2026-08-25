using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using school_management_system.DbModels;

namespace SchoolProject.Data;

public class SchoolDbContext : DbContext
{
    private readonly IConfiguration? _conf;

    public SchoolDbContext()
    {
    }

    public SchoolDbContext(IConfiguration conf) => _conf = conf;

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Class> Classes { get; set; } = null!;
    public DbSet<Student> Students { get; set; } = null!;
    public DbSet<Teacher> Teachers { get; set; } = null!;
    public DbSet<Discipline> Disciplines { get; set; } = null!;
    public DbSet<Schedule> Schedules { get; set; } = null!;
    public DbSet<Grade> Grades { get; set; } = null!;
    public DbSet<Attendance> Attendances { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured && _conf != null)
        {
            optionsBuilder.UseNpgsql(_conf["DBConnectionString"]);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 1. ПОЛЬЗОВАТЕЛИ
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users", "public");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
            entity.HasIndex(e => e.Name).IsUnique();
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash").HasMaxLength(255).IsRequired();
            entity.Property(e => e.Role).HasColumnName("role").HasMaxLength(20).HasConversion<string>();
        });

        // 2. УЧЕБНЫЕ КЛАССЫ
        modelBuilder.Entity<Class>(entity =>
        {
            entity.ToTable("classes", "public");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(10).IsRequired();
            entity.Property(e => e.AcademicYear).HasColumnName("academic_year").HasMaxLength(9).IsRequired();
            entity.HasIndex(e => new { e.Name, e.AcademicYear }).IsUnique();
        });

        // 3. УЧЕНИКИ
        modelBuilder.Entity<Student>(entity =>
        {
            entity.ToTable("students", "public");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FirstName).HasColumnName("first_name").HasMaxLength(50).IsRequired();
            entity.Property(e => e.LastName).HasColumnName("last_name").HasMaxLength(50).IsRequired();
            entity.Property(e => e.MiddleName).HasColumnName("middle_name").HasMaxLength(50);
            entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth").IsRequired();
            entity.Property(e => e.UserIdFk).HasColumnName("user_id_fk");
            entity.Property(e => e.ClassIdFk).HasColumnName("class_id_fk");

            entity.HasOne(d => d.User)
                .WithOne(p => p.Student)
                .HasForeignKey<Student>(d => d.UserIdFk)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Class)
                .WithMany(p => p.Students)
                .HasForeignKey(d => d.ClassIdFk)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // 4. УЧИТЕЛЯ
        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.ToTable("teachers", "public");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FirstName).HasColumnName("first_name").HasMaxLength(50).IsRequired();
            entity.Property(e => e.LastName).HasColumnName("last_name").HasMaxLength(50).IsRequired();
            entity.Property(e => e.MiddleName).HasColumnName("middle_name").HasMaxLength(50);
            entity.Property(e => e.UserIdFk).HasColumnName("user_id_fk");

            entity.HasOne(d => d.User)
                .WithOne(p => p.Teacher)
                .HasForeignKey<Teacher>(d => d.UserIdFk)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // 5. УЧЕБНЫЕ ДИСЦИПЛИНЫ
        modelBuilder.Entity<Discipline>(entity =>
        {
            entity.ToTable("disciplines", "public");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
            entity.HasIndex(e => e.Name).IsUnique();

            // 6. ДИСЦИПЛИНЫ УЧИТЕЛЕЙ (Промежуточная таблица)
            entity.HasMany(d => d.Teachers)
                .WithMany(p => p.Disciplines)
                .UsingEntity<Dictionary<string, object>>(
                    "teacher_disciplines",
                    r => r.HasOne<Teacher>().WithMany().HasForeignKey("teacher_id_fk").OnDelete(DeleteBehavior.Cascade),
                    l => l.HasOne<Discipline>().WithMany().HasForeignKey("discipline_id_fk").OnDelete(DeleteBehavior.Cascade),
                    je =>
                    {
                        je.ToTable("teacher_disciplines", "public");
                        je.HasKey("id");
                        je.Property<int>("id").ValueGeneratedOnAdd().HasColumnName("id");
                        je.Property<int>("teacher_id_fk").HasColumnName("teacher_id_fk");
                        je.Property<int>("discipline_id_fk").HasColumnName("discipline_id_fk");
                        je.HasIndex(new[] { "teacher_id_fk", "discipline_id_fk" }).IsUnique();
                    });
        });

        // 7. РАСПИСАНИЕ
        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.ToTable("schedule", "public");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClassIdFk).HasColumnName("class_id_fk");
            entity.Property(e => e.TeacherIdFk).HasColumnName("teacher_id_fk");
            entity.Property(e => e.DisciplineIdFk).HasColumnName("discipline_id_fk");
            entity.Property(e => e.DayOfWeek).HasColumnName("day_of_week").IsRequired();
            entity.Property(e => e.LessonNumber).HasColumnName("lesson_number").IsRequired();
            entity.Property(e => e.RoomNumber).HasColumnName("room_number").HasMaxLength(10).IsRequired();

            entity.HasOne(d => d.Class)
                .WithMany(p => p.Schedules)
                .HasForeignKey(d => d.ClassIdFk)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Teacher)
                .WithMany(p => p.Schedules)
                .HasForeignKey(d => d.TeacherIdFk)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.Discipline)
                .WithMany(p => p.Schedules)
                .HasForeignKey(d => d.DisciplineIdFk)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => new { e.ClassIdFk, e.DayOfWeek, e.LessonNumber }).IsUnique();
            entity.HasIndex(e => new { e.TeacherIdFk, e.DayOfWeek, e.LessonNumber }).IsUnique();
            entity.HasIndex(e => new { e.RoomNumber, e.DayOfWeek, e.LessonNumber }).IsUnique();
        });

        // 8. ОЦЕНКИ
        modelBuilder.Entity<Grade>(entity =>
        {
            entity.ToTable("grades", "public");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.StudentIdFk).HasColumnName("student_id_fk");
            entity.Property(e => e.TeacherIdFk).HasColumnName("teacher_id_fk");
            entity.Property(e => e.DisciplineIdFk).HasColumnName("discipline_id_fk");
            entity.Property(e => e.Value).HasColumnName("value").IsRequired();
            entity.Property(e => e.Date).HasColumnName("date").IsRequired();

            entity.HasOne(d => d.Student)
                .WithMany(p => p.Grades)
                .HasForeignKey(d => d.StudentIdFk)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Teacher)
                .WithMany(p => p.Grades)
                .HasForeignKey(d => d.TeacherIdFk)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.Discipline)
                .WithMany(p => p.Grades)
                .HasForeignKey(d => d.DisciplineIdFk)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // 9. ПОСЕЩАЕМОСТЬ
        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.ToTable("attendance", "public");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.StudentIdFk).HasColumnName("student_id_fk");
            entity.Property(e => e.ScheduleIdFk).HasColumnName("schedule_id_fk");
            entity.Property(e => e.Date).HasColumnName("date").IsRequired();
            entity.Property(e => e.Status).HasColumnName("status").HasMaxLength(20).HasConversion<string>();

            entity.HasOne(d => d.Student)
                .WithMany(p => p.Attendances)
                .HasForeignKey(d => d.StudentIdFk)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Schedule)
                .WithMany(p => p.Attendances)
                .HasForeignKey(d => d.ScheduleIdFk)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.StudentIdFk, e.ScheduleIdFk, e.Date }).IsUnique();
        });
    }
}