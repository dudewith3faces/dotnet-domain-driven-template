using Application.Common.Constants;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Behaviours;
/// <summary>
/// IpBehaviour checks the performance of each request. If the request take more than 500 milliseconds, it would log a warning. It also set the ip making the request to HttpContext.Item
/// </summary>
public class IpBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public IpBehaviour(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        string? ip = string.Empty;
        HttpContext httpContext = _httpContextAccessor.HttpContext!;

        ip = GetIpFromCsv(GetHeaderValueAs(IpConstant.IpHeaderForwarded));

        if (string.IsNullOrWhiteSpace(ip) && httpContext.Connection.RemoteIpAddress != null) ip = httpContext.Connection.RemoteIpAddress.ToString();

        if (string.IsNullOrWhiteSpace(ip)) ip = GetHeaderValueAs(IpConstant.IpHeaderRemote);

        httpContext.Items.Add(IpConstant.Ip, ip);

        return next();
    }

    private string? GetHeaderValueAs(string headerName)
    {
        string value = _httpContextAccessor.HttpContext!.Request.Headers[headerName];

        if (!string.IsNullOrWhiteSpace(value)) return (string)Convert.ChangeType(value.ToString(), typeof(string));

        return default(string);
    }

    private string? GetIpFromCsv(string? csvList)
    {
        if (string.IsNullOrWhiteSpace(csvList)) return default(string);

        return csvList
            .TrimEnd(',')
            .Split(',')
            .AsEnumerable<string>()
            .Select(s => s.Trim())
            .FirstOrDefault();
    }

}
