using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OldSchool.Application.Models;
using OldSchool.Domain.Models;
using OldSchool.Infrastructure;

namespace OldSchool.Application.Services;

public class AnalyticsService: IAnalyticsService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SchoolService> _logger;

    public AnalyticsService(
        ILogger<SchoolService> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task<SchoolAnalytics> GetStatistics()
    {
        
        var totalSchoolCountTask = _serviceProvider.GetRequiredService<IRepository<School>>().GetAll().CountAsync();
        var totalStudentCountTask = _serviceProvider.GetRequiredService<IRepository<Student>>().GetAll().CountAsync();
        var schoolAnalytics = new SchoolAnalytics();
        
        var averageScoreBySchoolTask = _serviceProvider.GetRequiredService<IRepository<School>>().GetAll().Select(h => new
        {
            Name = h.Name,
            AvgScore = h.Students.SelectMany(st => st.Scores.Select(f => f.Mark)).Average()
        }).ToListAsync();

        var studentCountByLocation = _serviceProvider.GetRequiredService<IRepository<School>>().GetAll().GroupBy(sc => sc.Location)
            .Select(h => new
            {
                Location = h.Key,
                Count = h.SelectMany(s => s.Students).Count()
            }).ToListAsync();
        await Task.WhenAll(totalSchoolCountTask, totalStudentCountTask,averageScoreBySchoolTask, studentCountByLocation);
        schoolAnalytics.TotalSchoolCount = totalSchoolCountTask.Result;
        schoolAnalytics.TotalStudentCount = totalStudentCountTask.Result;
        schoolAnalytics.AverageScoreBySchool = averageScoreBySchoolTask.Result.ToDictionary(scl => scl.Name, scr => scr.AvgScore);
        schoolAnalytics.StudentsCountByLocation = studentCountByLocation.Result.ToDictionary(loc => loc.Location, ct => ct.Count);
        return schoolAnalytics;
    }
}