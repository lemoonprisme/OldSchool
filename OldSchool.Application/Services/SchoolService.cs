using Bogus;
using Bogus.DataSets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OldSchool.Domain.Models;
using OldSchool.Infrastructure;

namespace OldSchool.Application.Services;

public class SchoolService: ISchoolService
{
    private readonly IRepository<School> _schoolRepository;
    private readonly IRepository<Student> _studentRepository;
    private readonly ILogger<SchoolService> _logger;

    public SchoolService(ILogger<SchoolService> logger, IRepository<School> schoolRepository, IRepository<Student> studentRepository)
    {
        _logger = logger;
        _schoolRepository = schoolRepository;
        _studentRepository = studentRepository;
    }

    public Task<List<string>> GetUniqueStudentNames()
    {
        _logger.LogInformation("Unique students names:");
        return _studentRepository.GetAll().Select(s => s.Name)
            .OrderBy(s => s).Distinct().ToListAsync();
    }

    public Dictionary<School, double> GetAverageStudentAge()
    {
        _logger.LogInformation("Average students age in school:");
        return _schoolRepository.GetAll().ToDictionary(d => d, s => s.Students.Select(f => f.Age).Average());
    }

    public Task<List<Student>> GetStudentsNameStartWithAAsync()
    {
        return _studentRepository.GetAll().Where(a => a.Name.StartsWith('А')).ToListAsync();
    }
    public async Task GenerateSchoolInStore()
    {
        var schoolFaker = new Faker<School>().RuleFor(s => s.Name, f => f.Internet.UserName())
            .RuleFor(s => s.Location, f => f.Internet.Ip());
        var school = schoolFaker.Generate();
        _schoolRepository.Create(school);
        await _schoolRepository.SaveAsync();
    }
    public async Task GenerateStudentInStore()
    {
        var studentFaker = new Faker<Student>().RuleFor(s => s.Name, f => f.Name.FullName())
            .RuleFor(s => s.Age, f => f.Random.Int(7, 20))
            .RuleFor(s => s.Gender, f => f.PickRandom<Gender>());
        var student = studentFaker.Generate();
        var schoolIds = _schoolRepository.GetAll().Select(s => s.SchoolId).ToArray();
            student.SchoolId = schoolIds.ElementAt(new Random().Next(0, schoolIds.Count()-1));
        _studentRepository.Create(student);
        await _studentRepository.SaveAsync();
        using (_logger.BeginScope(new Dictionary<string, object>()
               {
                   {"@Student", student}
               }))
        _logger.LogInformation("Student created(Id:{StudentId})", student.StudentId);
    }

    public IEnumerable<School> GetSchools() => _schoolRepository.GetAll();
}