using OldSchool.Domain.Models;
using OldSchool.Infrastructure;

namespace OldSchool.Application.Models;

public class SchoolAnalytics
{
    public int TotalSchoolCount { get; set; }
    public int TotalStudentCount { get; set; }
    public Dictionary<string, int> StudentsCountByLocation { get; set; } = default!;
    public Dictionary<string, string> LocationWithHighestScoreBySubject { get; set; } = default!;
    public Dictionary<string, double> AverageScoreBySchool { get; set; } = default!;
}