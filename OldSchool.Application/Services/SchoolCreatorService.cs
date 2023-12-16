using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace OldSchool.Application.Services;

public class SchoolCreatorService : IHostedService
{
    private readonly ISchoolService _schoolService;
    private readonly ILogger<SchoolCreatorService> _logger;

    public SchoolCreatorService(ISchoolService schoolService, ILogger<SchoolCreatorService> logger)
    {
        _schoolService = schoolService;
        _logger = logger;
    }
    protected async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        
            try
            {
                await _schoolService.GenerateSchoolInStore();
            }
            catch (Exception e)
            {
                _logger.LogError(e,"Can't create a school.");
            }
            
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _schoolService.GenerateSchoolInStore();
        }
        catch (Exception e)
        {
            _logger.LogError(e,"Can't create a school.");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}