using Microsoft.EntityFrameworkCore;
using OldSchool.Domain.Models;

namespace OldSchool.Infrastructure;

public class SchoolDataBase : ISchoolStore
{
    private readonly ApplicationContext _context;

    public SchoolDataBase(ApplicationContext applicationContext)
    {
        _context = applicationContext;
    }
    public IEnumerable<School> GetSchools()
    {
        return _context.Schools.Include(s => s.Students).ToList();
    }

    public void AddSchool(School school)
    {
        _context.Schools.Add(school);
        _context.SaveChanges();
    }

    public void AddStudent(Student student)
    {
        _context.Students.Add(student);
        _context.SaveChanges();
    }

    public List<int> GetSchoolIds()
    {
        return _context.Schools.Select(s => s.SchoolId).ToList();
    }
}