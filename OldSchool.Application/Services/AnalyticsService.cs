using Microsoft.Extensions.Logging;
using OldSchool.Application.Models;
using OldSchool.Domain.Models;
using OldSchool.Infrastructure;

namespace OldSchool.Application.Services;

public class AnalyticsService: IAnalyticsService
{
    private readonly IRepository<School> _schoolRepository;
    private readonly IRepository<Student> _studentRepository;
    private readonly ILogger<SchoolService> _logger;

    public AnalyticsService(IRepository<School> schoolRepository, IRepository<Student> studentRepository, ILogger<SchoolService> logger)
    {
        _schoolRepository = schoolRepository;
        _studentRepository = studentRepository;
        _logger = logger;
    }

    public SchoolAnalytics GetStatistics()
    {
        var schoolAnalytics = new SchoolAnalytics();
        schoolAnalytics.TotalSchoolCount = _schoolRepository.GetAll().Count();
        schoolAnalytics.TotalStudentCount = _studentRepository.GetAll().Count();
        schoolAnalytics.AverageScoreBySchool = _schoolRepository.GetAll().Select(h => new
        {
            Name = h.Name,
            AvgScore = h.Students.SelectMany(st => st.Scores.Select(f => f.Mark)).Average()
        }).AsEnumerable().ToDictionary(scl => scl.Name, scr => scr.AvgScore);
        

        return schoolAnalytics;
    }
}