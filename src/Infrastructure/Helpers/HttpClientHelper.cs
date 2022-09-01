using System.Net;
using System.Net.Http;
using System.Text;
using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Extensions;

namespace Infrastructure.Helpers;

public class HttpClientHelper
{
    private readonly HttpClient _client;
    private readonly IDictionary<string, string> _headers;
    public HttpClientHelper(IHttpClientFactory factory)
    {
        _client = factory.CreateClient("BaseClient");
        _headers = new Dictionary<string, string>();
    }
    protected async Task<T> PostAsync<T>(string url, object body, CancellationToken cancellationToken)
    {
        var req = BuildRequest(HttpMethod.Post, url);

        req.Content = new StringContent(body.Serialize(), Encoding.UTF8, "application/json");

        return await HandleResponse<T>(req, cancellationToken);
    }
    protected async Task<T> GetAsync<T>(string url, CancellationToken cancellationToken)
    {
        var req = BuildRequest(HttpMethod.Get, url);

        return await HandleResponse<T>(req, cancellationToken);
    }
    protected string CreateUrl(string baseUrl, params string[] path)
    {
        if (string.IsNullOrEmpty(baseUrl)) throw new ArgumentNullException();
        if (path == null || path.Length == 0) return baseUrl;

        baseUrl = $"{baseUrl.Trim('/')}/{path.First().Trim('/')}";
        return CreateUrl(baseUrl, path.Skip(1).ToArray());
    }
    protected virtual object ExceptionObjectFormater(string body)
    {
        return body.Deserialize<IDictionary<string, dynamic>>();
    }
    protected void AddHeader(string key, string value) => _headers.Add(key, value);
    private async Task<T> HandleResponse<T>(HttpRequestMessage req, CancellationToken cancellationToken)
    {
        var res = await _client.SendAsync(req, cancellationToken);
        var body = await res.Content.ReadAsStringAsync(cancellationToken);
        if (!res.IsSuccessStatusCode)
        {
            // if (res.StatusCode < HttpStatusCode.InternalServerError) ExceptionHandler.HandleException(ExceptionObjectFormater(body));
            // else ExceptionHandler.HandleException();
        };
        return body.Deserialize<T>();
    }
    private HttpRequestMessage BuildRequest(HttpMethod method, string url)
    {
        HttpRequestMessage req = new HttpRequestMessage(method, url);

        _headers.ToList().ForEach(x => req.Headers.Add(x.Key, x.Value));

        return req;
    }
}