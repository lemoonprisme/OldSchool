using OldSchool.Domain.Models;

namespace OldSchool.Application.Tests;

public class StudentBuilder
{
    private Student _student;

    public StudentBuilder()
    {
        _student = new Student();
        _student.SchoolId = 1;
        _student.StudentId = 1;
        _student.Name = "Ivan Braiko";
        _student.Age = 13;
        _student.Gender = Gender.Male;
        _student.Scores = new List<Score>()
        {
            new Score()
            {
                Mark = 5,
                Subject = "Inf",
                ScoreId = 1,
                StudentId = 1
            }
        };
    }

    public StudentBuilder SetStudentId(int id)
    {
        _student.StudentId = id;
        return this;
    }
    public StudentBuilder SetSchoolId(int id)
    {
        _student.SchoolId = id;
        return this;
    }
    public StudentBuilder SetStudentName(string name)
    {
        _student.Name = name;
        return this;
    }
    public StudentBuilder SetStudentAge(int age)
    {
        _student.StudentId = age;
        return this;
    }
    public StudentBuilder SetGenderId(Gender gender)
    {
        _student.Gender = gender;
        return this;
    }
    public StudentBuilder SetStudentScores(List<Score> scores)
    {
        _student.Scores = scores;
        return this;
    }

    public Student Build()
    {
        return _student;
    }
}