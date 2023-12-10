using Microsoft.EntityFrameworkCore;
using OldSchool.Domain.Models;

namespace OldSchool.Infrastructure;

public class ApplicationContext : DbContext
{
    private readonly string _connectingString;
    public DbSet<School> Schools { get; set; }
    public DbSet<Student> Students { get; set; }
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var schoolId = 1;
        var student1 = new Student()
        {
            StudentId = 1,
            Age = 16,
            Name = "John Ube",
            SchoolId = schoolId
        };
        var student2 = new Student()
        {
            StudentId = 2,
            Age = 17,
            Name = "Karl Uber",
            SchoolId = schoolId
        };
        modelBuilder.Entity<Student>().HasData(student1,student2);
        var school = new School
        {
            SchoolId = schoolId,
            Name = "William's Hight School",
            Location = "Shakespeareland",
        };
        modelBuilder.Entity<School>().HasMany(u => u.Students);
        modelBuilder.Entity<School>().HasData(school);

    }
}