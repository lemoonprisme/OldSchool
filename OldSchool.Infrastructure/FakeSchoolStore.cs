using OldSchool.Domain.Models;

namespace OldSchool.Infrastructure;

public class FakeSchoolStore: ISchoolStore
{
    public IEnumerable<School> GetSchools()
    {
        // Создание школ
return  new List<School>
{
    new School
    {
        SchoolId = 1,
        Name = "Школа A",
        Location = "Город A",
        Students = new List<Student>
        {
            new Student { StudentId = 1, Name = "Алиса", Age = 15, SchoolId = 1 },
            new Student { StudentId = 2, Name = "Боб", Age = 16, SchoolId = 1 },
            new Student { StudentId = 3, Name = "Кэрол", Age = 14, SchoolId = 1 }
        }
    },
    new School
    {
        SchoolId = 2,
        Name = "Школа B",
        Location = "Город B",
        Students = new List<Student>
        {
            new Student { StudentId = 4, Name = "Дэвид", Age = 15, SchoolId = 2 },
            new Student { StudentId = 5, Name = "Эмили", Age = 16, SchoolId = 2 },
            new Student { StudentId = 20, Name = "Оливер", Age = 14, SchoolId = 5 },
            new Student { StudentId = 21, Name = "Алиса", Age = 15, SchoolId = 1 },
            new Student { StudentId = 6, Name = "Фрэнк", Age = 14, SchoolId = 2 },
            new Student { StudentId = 7, Name = "Грейс", Age = 15, SchoolId = 2 }
        }
    },
    new School
    {
        SchoolId = 3,
        Name = "Школа C",
        Location = "Город C",
        Students = new List<Student>
        {
            new Student { StudentId = 8, Name = "Ханна", Age = 15, SchoolId = 3 },
            new Student { StudentId = 9, Name = "Айзек", Age = 16, SchoolId = 3 },
            new Student { StudentId = 22, Name = "Оливер", Age = 14, SchoolId = 5 },
            new Student { StudentId = 10, Name = "Джейн", Age = 14, SchoolId = 3 },
            new Student { StudentId = 23, Name = "Алиса", Age = 15, SchoolId = 1 },
            new Student { StudentId = 11, Name = "Кевин", Age = 15, SchoolId = 3 },
            new Student { StudentId = 12, Name = "Линда", Age = 14, SchoolId = 3 }
        }
    },
    new School
    {
        SchoolId = 4,
        Name = "Школа D",
        Location = "Город D",
        Students = new List<Student>
        {
            new Student { StudentId = 13, Name = "Майкл", Age = 15, SchoolId = 4 }
        }
    },
    new School
    {
        SchoolId = 5,
        Name = "Школа E",
        Location = "Город E",
        Students = new List<Student>
        {
            new Student { StudentId = 14, Name = "Нора", Age = 16, SchoolId = 5 },
            new Student { StudentId = 15, Name = "Оливер", Age = 14, SchoolId = 5 },
            new Student { StudentId = 16, Name = "Пэт", Age = 15, SchoolId = 5 },
            new Student { StudentId = 17, Name = "Куин", Age = 14, SchoolId = 5 },
            new Student { StudentId = 24, Name = "Алиса", Age = 15, SchoolId = 1 },
            new Student { StudentId = 18, Name = "Райли", Age = 15, SchoolId = 5 },
            new Student { StudentId = 19, Name = "Стив", Age = 16 }
        }
    }
};

    }

    public void AddSchool(School school)
    {
        throw new NotImplementedException();
    }

    public void AddStudent(Student student)
    {
        throw new NotImplementedException();
    }

    public List<int> GetSchoolIds()
    {
        throw new NotImplementedException();
    }
}