using OldSchool.Domain.Models;

namespace OldSchool.Infrastructure;

public class School
{

    public int SchoolId { get; set; }

    public string Name { get; set; }
    public string Location { get; set; }
    public List<Student> Students { get; set; }
}