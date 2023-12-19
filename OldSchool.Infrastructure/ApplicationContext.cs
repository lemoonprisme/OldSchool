using Microsoft.EntityFrameworkCore;
using OldSchool.Domain.Models;

namespace OldSchool.Infrastructure;

public class ApplicationContext : DbContext
{
    private readonly string _connectingString;
    public DbSet<School> Schools { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Score> Scores { get; set; }
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var schoolId = 1;


        var scores1 = new List<Score>()
        {
            new Score("Математика", 4.0, 1,1),
            new Score("Литература", 4.0, 1,2)
        };
        var scores2 = new List<Score>()
        {
            new Score("Математика", 5.0, 2,3),
            new Score("Литература", 5.0, 2, 4)
        };
        
        modelBuilder.Entity<Score>().HasKey(s => s.ScoreId);
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
        
        var school = new School
        {
            SchoolId = schoolId,
            Name = "William's Hight School",
            Location = "Shakespeareland"
        };

    }
}