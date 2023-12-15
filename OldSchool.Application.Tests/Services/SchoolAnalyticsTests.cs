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
                             new Score("Математика", 4.5, 1,1),
                             new Score("Литература", 4.0, 1,2)
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
                             new Score("Математика", 4.0, 2,3),
                             new Score("Литература", 4.5, 2, 4)
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
                             new Score("Математика", 3.0,3, 5),
                             new Score("Литература", 3.0, 3, 6)
                         }
                     },
                     new Student
                     {
                         StudentId = 4,
                         Name = "Яна",
                         Age = 16,
                         SchoolId = 2,
                         Gender = Gender.Female,
                         Scores = new List<Score>
                         {
                             new Score("Математика", 3.0, 4,7),
                             new Score("Литература", 5.0, 4, 8)
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
                         StudentId = 5,
                         Name = "Максим",
                         Age = 15,
                         SchoolId = 2,
                         Gender = Gender.Male,
                         Scores = new List<Score>
                         {
                             new Score("Математика", 3.0, 5,9),
                             new Score("Литература", 3.0, 5,10)
                         }
                     },
                     new Student
                     {
                         StudentId = 6,
                         Name = "Яна",
                         Age = 16,
                         SchoolId = 2,
                         Gender = Gender.Female,
                         Scores = new List<Score>
                         {
                             new Score("Математика", 3.0, 6, 11),
                             new Score("Литература", 5.0, 6, 12)
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