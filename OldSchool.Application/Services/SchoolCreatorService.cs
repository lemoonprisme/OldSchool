using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace OldSchool.Application.Services;

public class SchoolCreatorService : IHostedService
{
    private readonly ILogger<SchoolCreatorService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public SchoolCreatorService(ILogger<SchoolCreatorService> logger, 
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _serviceProvider.GetRequiredService<ISchoolService>().GenerateSchoolInStore();
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