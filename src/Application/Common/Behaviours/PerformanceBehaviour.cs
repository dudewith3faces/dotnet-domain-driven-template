using System.Diagnostics;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Behaviours;
/// <summary>
/// PerformanceBehaviour checks the performance of each request. If the request take more than 500 milliseconds, it would log a warning. It also set the ip making the request to HttpContext.Item
/// </summary>
public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly Stopwatch _timer;
    private readonly ILoggerService _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public PerformanceBehaviour(ILoggerService logger, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _timer = new Stopwatch();
        _logger = logger;
    }
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        _timer.Start();

        var response = await next();

        _timer.Stop();

        long elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > 500) _logger.Warning("Notification Service Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@Request}", typeof(TResponse).Name, elapsedMilliseconds, request!);

        return response;
    }

}
