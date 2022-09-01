using Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

internal class LoggerService : ILoggerService
{
    private readonly ILogger _logger;
    public LoggerService(ILogger<LoggerService> logger)
    {
        _logger = logger;
    }

    public void Error(string data, params object[] args)
    {
        _logger.LogError(data, args);
    }

    public void Error(Exception ex, params object[] args)
    {
        _logger.LogWarning(ex.Message, ex, args);
    }

    public void Info(string data, params object[] args)
    {
        _logger.LogInformation(data, args);
    }

    public void Warning(string data, params object[] args)
    {
        _logger.LogWarning(data, args);
    }
}