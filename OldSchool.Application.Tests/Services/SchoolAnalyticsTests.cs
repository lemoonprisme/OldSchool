using Argon;
using Bogus;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using OldSchool.Application.Services;
using OldSchool.Domain.Models;
using OldSchool.Infrastructure;

namespace OldSchool.Application.Tests.Services;
[UsesVerify]
public class SchoolAnalyticsTests
{
    [Fact]
    public Task GetAnalytics_ReturnStatistics()
    {
        //Arrange
        var mockStudents = new Mock<IRepository<Student>>();
        var mockSchools = new Mock<IRepository<School>>();
        
        
         // Создание школ
         
         List<School> schools = new List<School>
         {
             new School
             {
                 SchoolId = 1,
                 Name = "Школа №1",
                 Location = "Город А",
                 Students = new List<Student>
                 {
                     new Student
                     {
                         StudentId = 1,
                         Name = "Иван",
                         Age = 15,
                         SchoolId = 1,
                         Gender = Gender.Male,
                         Scores = new List<Score>
                         {
                             new Score("Математика", 4.5),
                             new Score("Литература", 4.0)
                         }
                     },
                     new Student
                     {
                         StudentId = 2,
                         Name = "Мария",
                         Age = 16,
                         SchoolId = 1,
                         Gender = Gender.Female,
                         Scores = new List<Score>
                         {
                             new Score("Математика", 4.0),
                             new Score("Литература", 4.5)
                         }
                     }
                 }
             },
             // Добавляем другие школы здесь...
             new School
             {
                 SchoolId = 2,
                 Name = "Школа №2",
                 Location = "Город Б",
                 Students = new List<Student>
                 {
                     new Student
                     {
                         StudentId = 3,
                         Name = "Максим",
                         Age = 15,
                         SchoolId = 2,
                         Gender = Gender.Male,
                         Scores = new List<Score>
                         {
                             new Score("Математика", 3.0),
                             new Score("Литература", 3.0)
                         }
                     },
                     new Student
                     {
                         StudentId = 3,
                         Name = "Яна",
                         Age = 16,
                         SchoolId = 2,
                         Gender = Gender.Female,
                         Scores = new List<Score>
                         {
                             new Score("Математика", 3.0),
                             new Score("Литература", 5.0)
                         }
                     }
                 }
             },
             new School
             {
                 SchoolId = 3,
                 Name = "Школа №3",
                 Location = "Город А",
                 Students = new List<Student>
                 {
                     new Student
                     {
                         StudentId = 3,
                         Name = "Максим",
                         Age = 15,
                         SchoolId = 2,
                         Gender = Gender.Male,
                         Scores = new List<Score>
                         {
                             new Score("Математика", 3.0),
                             new Score("Литература", 3.0)
                         }
                     },
                     new Student
                     {
                         StudentId = 3,
                         Name = "Яна",
                         Age = 16,
                         SchoolId = 2,
                         Gender = Gender.Female,
                         Scores = new List<Score>
                         {
                             new Score("Математика", 3.0),
                             new Score("Литература", 5.0)
                         }
                     }
                 }
             }
         };
         
        mockStudents.Setup(a => a.GetAll()).Returns(schools.SelectMany(s => s.Students));
        mockSchools.Setup(a => a.GetAll()).Returns(schools);
        var service = new AnalyticsService(mockSchools.Object, mockStudents.Object, new NullLogger<SchoolService>());
        //Act
        var statistics = service.GetStatistics();
        
        
        //Assert
        return Verify(statistics);
    }
}