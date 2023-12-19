using OldSchool.Domain.Models;
using OldSchool.Infrastructure;

namespace OldSchool.Application.Services;

public interface ISchoolService
{
    Task<List<string>> GetUniqueStudentNames();
    Dictionary<School, double> GetAverageStudentAge();
    Task<List<Student>> GetStudentsNameStartWithAAsync();
    Task GenerateStudentInStore();
    public Task GenerateSchoolInStore();
}