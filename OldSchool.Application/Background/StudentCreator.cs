using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OldSchool.Application.Services;
using OldSchool.Domain.Models;
using OldSchool.Infrastructure;

namespace OldSchool.Application.Background;

public class StudentCreator : BackgroundService
{
    private readonly IRepository<School> _schoolRepository;
    private readonly IRepository<Student> _studentRepository;
    private readonly ILogger<SchoolService> _logger;

    public StudentCreator(ILogger<SchoolService> logger, IRepository<School> schoolRepository,
        IRepository<Student> studentRepository)
    {
        _logger = logger;
        _schoolRepository = schoolRepository;
        _studentRepository = studentRepository;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {

        return Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var studentFaker = new Faker<Student>().RuleFor(s => s.Name, f => f.Name.FullName())
                        .RuleFor(s => s.Age, f => f.Random.Int(7, 20))
                        .RuleFor(s => s.Gender, f => f.PickRandom<Gender>());
                    var student = studentFaker.Generate();
                    var schoolIds = await _schoolRepository.GetAll().Select(s => s.SchoolId).ToArrayAsync(cancellationToken: stoppingToken);
                    student.SchoolId = schoolIds.ElementAt(new Random().Next(0, schoolIds.Count()-1));
                    _studentRepository.Create(student);
                    await _studentRepository.SaveAsync();
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