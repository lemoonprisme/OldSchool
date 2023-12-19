using OldSchool.Domain.Models;
using OldSchool.Infrastructure;

namespace OldSchool.Application.Tests;

public class SchoolBuilder
{
    private School _school;

    public SchoolBuilder()
    {
        _school = new School();
        _school.SchoolId = 1;
        _school.Location = "Russia";
        _school.Name = "School";
    }

    public SchoolBuilder SetSchoolId(int schoolId)
    {
        _school.SchoolId = schoolId;
        return this;
    }

    public SchoolBuilder SetSchoolName(string schoolName)
    {
        _school.Name = schoolName;
        return this;
    }

    public SchoolBuilder SetLocation(string location)
    {
        _school.Location = location;
        return this;
    }

    public SchoolBuilder SetStudents(List<Student> students)
    {
        _school.Students = students;
        return this;
    }

    public School Build()
    {
        
        return _school;
    }
}