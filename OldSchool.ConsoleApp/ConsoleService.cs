using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OldSchool.Application.Background;
using OldSchool.Application.Services;
using OldSchool.Domain.Models;
using OldSchool.Infrastructure;

namespace OldSchool.ConsoleApp;

public class ConsoleService : IHostedService
{
    private readonly IRepository<School> _schoolRepository;
    private readonly IRepository<Student> _studentRepository;
    private readonly ILogger<SchoolService> _logger;
    private readonly ISchoolService _schoolService;
    private readonly IAnalyticsService _analyticsService;
    public ConsoleService(ILogger<SchoolService> logger, IRepository<School> schoolRepository,
        IRepository<Student> studentRepository, ISchoolService schoolService, IAnalyticsService analyticsService)
    {
        _logger = logger;
        _schoolRepository = schoolRepository;
        _studentRepository = studentRepository;
        _schoolService = schoolService;
        _analyticsService = analyticsService;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _schoolService.GenerateSchoolInStore();
        while (!cancellationToken.IsCancellationRequested)
        {
            
            var message = Console.ReadLine();
            if (!String.IsNullOrEmpty(message))
            {
                message = message.ToLower();
            }
            switch (message)
            {
                case "get analytics":
                    await _analyticsService.GetStatistics();
                    break;
                case "stop":
                    return;
                    break;
                case "enough":
                    
                    break;
                case null:
                    break;
                case "generate a school":
                {
                    await _schoolService.GenerateSchoolInStore();
                    break;
                }
                case "get students":
                {
                    Console.Write("Write school Id: ");
                    var schoolIdString = Console.ReadLine();
                    int schoolId;
                    if (int.TryParse(schoolIdString, out schoolId))
                    {
                        var studentsNames = _schoolRepository.GetAll() 
                            .Where(s => s.SchoolId == schoolId)
                            .SelectMany(s => s.Students.Select(f => f.Name));
                        foreach (var name in studentsNames)
                        {
                            Console.WriteLine(name);
                        }
                
                    }
                    break;
                }
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}