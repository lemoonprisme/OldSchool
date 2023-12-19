using Argon;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using MockQueryable.Moq;
using Moq;
using OldSchool.Application.Models;
using OldSchool.Application.Services;
using OldSchool.Domain.Models;
using OldSchool.Infrastructure;

namespace OldSchool.Application.Tests.Services;
[UsesVerify]
public class SchoolAnalyticsTests
{
    [Fact]
    public async Task GetAnalytics_ReturnStatistics()
    {
        //Arrange
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
        
        var schoolsMock = schools.BuildMock();
        
        ServiceCollection sc = new ServiceCollection();
        var mockStudents = new Mock<IRepository<Student>>();
        var mockSchools = new Mock<IRepository<School>>();
        
        mockSchools.Setup(s => s.GetAll())
            .Returns(schoolsMock);
        mockStudents.Setup(s => s.GetAll())
            .Returns(schoolsMock.SelectMany(s => s.Students));
        sc.AddScoped<IRepository<Student>>(_ => mockStudents.Object);
        sc.AddScoped<IRepository<School>>(_ => mockSchools.Object);
        var serviceProvider = sc.BuildServiceProvider();
         // Создание школ
        
        var service = new AnalyticsService( new NullLogger<SchoolService>(), serviceProvider);
        //Act
        var statistics = await service.GetStatistics();
        
        
        //Assert
        await Verify(statistics);
    }
    [Fact]
    public async Task GetAnalytics_OneStudentWithoutScores_ReturnStatistics()
    {
        //Arrange
        var schools = new List<School>()
        {
            new SchoolBuilder().SetSchoolName("Alpha").SetStudents(new List<Student>()
            {
                new StudentBuilder().SetSchoolId(1).Build(),
                new StudentBuilder().SetSchoolId(1).SetStudentScores(new List<Score>()
                {
                    new ScoreBuilder().SetMark(4).SetSubject("Inf").SetMark(3).Build()
                }).Build()
            }).Build(),
            new SchoolBuilder().SetSchoolName("Beta").SetStudents(new List<Student>()
            {
                new StudentBuilder().SetSchoolId(2).Build(),
                new StudentBuilder().SetSchoolId(2).SetStudentScores(new List<Score>()
                {
                    new ScoreBuilder().SetMark(4).SetSubject("Inf").SetMark(3).Build(),
                    new ScoreBuilder().SetMark(3).Build()
                }).Build()
            }).Build()
        };
        var schoolsMock = schools.BuildMock();
        
        ServiceCollection sc = new ServiceCollection();
        var mockStudents = new Mock<IRepository<Student>>();
        var mockSchools = new Mock<IRepository<School>>();
        
        mockSchools.Setup(s => s.GetAll())
            .Returns(schoolsMock);
        mockStudents.Setup(s => s.GetAll())
            .Returns(schoolsMock.SelectMany(s => s.Students));
        sc.AddScoped<IRepository<Student>>(_ => mockStudents.Object);
        sc.AddScoped<IRepository<School>>(_ => mockSchools.Object);
        var serviceProvider = sc.BuildServiceProvider();
         // Создание школ
        
        var service = new AnalyticsService( new NullLogger<SchoolService>(), serviceProvider);
        //Act
        var statistics = await service.GetStatistics();
        
        
        //Assert
        await Verify(statistics);
    }
}