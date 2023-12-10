using Bogus;
using Microsoft.Extensions.Logging;
using OldSchool.Domain.Models;
using OldSchool.Infrastructure;

namespace OldSchool.Application.Services;

public class SchoolService: ISchoolService
{
    private readonly ISchoolStore _schoolStore;
    private readonly ILogger<SchoolService> _logger;

    public SchoolService(ISchoolStore schoolStore, ILogger<SchoolService> logger)
    {
        _schoolStore = schoolStore;
        _logger = logger;
    }

    public IEnumerable<string> GetUniqueStudentNames()
    {
        _logger.LogInformation("Unique students names:");
        return _schoolStore.GetSchools().SelectMany(s => s.Students).Select(s => s.Name)
            .OrderBy(s => s).Distinct();
    }

    public Dictionary<School, double> GetAverageStudentAge()
    {
        _logger.LogInformation("Average students age in school:");
        return _schoolStore.GetSchools().ToDictionary(d => d, s => s.Students.Select(f => f.Age).Average());
    }

    public IEnumerable<Student> GetStudentsNameStartWithA()
    {
        return _schoolStore.GetSchools().SelectMany(a => a.Students).Where(a => a.Name.StartsWith('А'));
    }
    public void GenerateSchoolInStore()
    {
        var schoolFaker = new Faker<School>().RuleFor(s => s.Name, f => f.Internet.UserName())
            .RuleFor(s => s.Location, f => f.Internet.Ip());
        var school = schoolFaker.Generate();
        var studentFaker = new Faker<Student>().RuleFor(s => s.Name, f => f.Name.FullName());
        var students = studentFaker.Generate(new Random().Next(0, 100));
        school.Students = students;
        _schoolStore.AddSchool(school);
        
    }
    public void GenerateStudentInStore()
    {
        var studentFaker = new Faker<Student>().RuleFor(s => s.Name, f => f.Name.FullName());
        var student = studentFaker.Generate();
        var schoolIds = _schoolStore.GetSchoolIds();
        student.SchoolId = schoolIds.ElementAt(new Random().Next(0, schoolIds.Count-1));
        _schoolStore.AddStudent(student);
        using (_logger.BeginScope(new Dictionary<string, object>()
               {
                   {"@Student", student}
               }))
        _logger.LogInformation("Student created(Id:{StudentId})", student.StudentId);
    }

    public IEnumerable<School> GetSchools() => _schoolStore.GetSchools();
}