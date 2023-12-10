using OldSchool.Domain.Models;

namespace OldSchool.Infrastructure;

public interface ISchoolStore
{
    public IEnumerable<School> GetSchools();
    public void AddSchool(School school);
    public void AddStudent(Student student);
    public List<int> GetSchoolIds();
}