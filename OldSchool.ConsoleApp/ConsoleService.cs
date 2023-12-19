using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OldSchool.Application.Background;
using OldSchool.Application.Services;
using OldSchool.Domain.Models;
using OldSchool.Infrastructure;

namespace OldSchool.ConsoleApp;

public class ConsoleService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SchoolService> _logger;
    private readonly IAnalyticsService _analyticsService;
    public ConsoleService(ILogger<SchoolService> logger,
        IAnalyticsService analyticsService, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _analyticsService = analyticsService;
        _serviceProvider = serviceProvider;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
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
                    var analytics = await _analyticsService.GetStatistics();
                    Console.WriteLine($"Всего школ:{analytics.TotalSchoolCount}");
                    Console.WriteLine($"Всего учеников: {analytics.TotalStudentCount}");
                    Console.WriteLine("Средний балл по школам:");
                    foreach (var score in analytics.AverageScoreBySchool)
                    {
                        Console.WriteLine($"{score.Key} {score.Value}");
                    }
                    Console.WriteLine("Локация с числом студентов в ней: ");
                    foreach (var count in analytics.StudentsCountByLocation)
                    {
                        Console.WriteLine($"{count.Key} {count.Value}");
                    }
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
                    await _serviceProvider.GetRequiredService<ISchoolService>().GenerateSchoolInStore();
                    break;
                }
                case "get students":
                {
                    Console.Write("Write school Id: ");
                    var schoolIdString = Console.ReadLine();
                    int schoolId;
                    if (int.TryParse(schoolIdString, out schoolId))
                    {
                        var studentsNames = _serviceProvider.GetRequiredService<IRepository<School>>().GetAll() 
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