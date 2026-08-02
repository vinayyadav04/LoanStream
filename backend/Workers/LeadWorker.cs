using Microsoft.Extensions.Logging;

namespace LoanStream.Api.Workers;

public sealed class LeadWorker : BackgroundService
{
    private readonly ILogger<LeadWorker> _logger;

    public LeadWorker(ILogger<LeadWorker> logger)
    {
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Lead background processing is disabled; leads are stored directly.");
        return Task.CompletedTask;
    }
}
