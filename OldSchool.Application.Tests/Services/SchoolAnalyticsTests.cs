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
         var schools = new List<School>();
         var subjects = new[] { "QM", "FT", "NPh" };
         for (var i = 0; i < 5; i++)
         {

             var scoreFaker = new Faker<Score>().CustomInstantiator(f => new Score(
                 f.PickRandom(subjects),
                 f.Random.Double(0, 5)
                 )
             );
             var schoolFaker = new Faker<School>().RuleFor(s => s.Name, f => f.Internet.UserName())
                 .RuleFor(s => s.Location, f => f.Internet.Ip());
             var school = schoolFaker.Generate();
             var studentFaker = new Faker<Student>().RuleFor(s => s.Name, f => f.Name.FullName())
                 .RuleFor(o=>o.Scores,f => scoreFaker.Generate(new Random().Next(1,10)));
             var students = studentFaker.Generate(new Random().Next(0, 100));
             school.Students = students;
             schools.Add(school);
         }
            
        mockStudents.Setup(a => a.GetAll()).Returns(schools.SelectMany(s => s.Students));
        mockSchools.Setup(a => a.GetAll()).Returns(schools);
        var service = new AnalyticsService(mockSchools.Object, mockStudents.Object, new NullLogger<SchoolService>());
        //Act
        var statistics = service.GetStatistics();
        
        
        //Assert
        return Verify($"School count: {statistics.TotalSchoolCount}\nStudent count: {statistics.TotalStudentCount}\n{JsonConvert.SerializeObject(statistics.AverageScoreBySchool)}");
    }
}