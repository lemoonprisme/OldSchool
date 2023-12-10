using OldSchool.Domain.Models;
using OldSchool.Infrastructure;

namespace OldSchool.Application.Services;

public interface ISchoolService
{
    IEnumerable<string> GetUniqueStudentNames();
    Dictionary<School, double> GetAverageStudentAge();
    IEnumerable<Student> GetStudentsNameStartWithA();
    void GenerateStudentInStore();
}