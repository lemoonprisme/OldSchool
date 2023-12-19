using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OldSchool.Application.Services;
using OldSchool.Domain.Models;
using OldSchool.Infrastructure;

namespace OldSchool.Application.Background;

public class StudentCreator : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SchoolService> _logger;

    public StudentCreator(ILogger<SchoolService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {

        return Task.Run(async () =>
        {
            var schoolRepositoryService = _serviceProvider.GetRequiredService<IRepository<School>>();
            var studentRepositoryService = _serviceProvider.GetRequiredService<IRepository<Student>>();
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var subjects = new List<string>() { "Матан", "Литра" };
                    var scoreFaker = new Faker<Score>().RuleFor(s => s.Mark, f => f.Random.Int(0, 5))
                        .RuleFor(s => s.Subject, f => f.PickRandom(subjects));
                    var studentFaker = new Faker<Student>().RuleFor(s => s.Name, f => f.Name.FullName())
                        .RuleFor(s => s.Age, f => f.Random.Int(7, 20))
                        .RuleFor(s => s.Gender, f => f.PickRandom<Gender>())
                        .RuleFor(s=>s.Scores,f=> scoreFaker.Generate(3));
                    var student = studentFaker.Generate();
                    var schoolIds = await schoolRepositoryService.GetAll().Select(s => s.SchoolId).
                        ToArrayAsync(cancellationToken: stoppingToken);
                    student.SchoolId = schoolIds.ElementAt(new Random().Next(0, schoolIds.Count()));
                    studentRepositoryService.Create(student);
                    await studentRepositoryService.SaveAsync();
                    // using (_logger.BeginScope(new Dictionary<string, object>()
                    //        {
                    //            {"@Student", student}
                    //        }))
                    //     _logger.LogInformation("Student created(Id:{StudentId})", student.StudentId);
                }
                catch (OperationCanceledException exception) when (exception.CancellationToken == stoppingToken)
                {
                    return;
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Can't create a student");
                }
                Thread.Sleep(1000);
            }
        }, stoppingToken);
}
}