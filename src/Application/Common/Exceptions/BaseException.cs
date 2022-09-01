using System.Net;

namespace Application.Common.Exceptions;

public class BaseException : System.Exception
{
    public BaseException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest) : base(message)
    {
        Payload = new { };
        SetHttpStatusCode((int)statusCode);
    }
    public BaseException(string message, int statusCode = 400) : base(message)
    {
        Payload = new { };
        SetHttpStatusCode(statusCode);
    }

    public BaseException(string message, object payload, HttpStatusCode statusCode = HttpStatusCode.BadRequest) : base(message)
    {
        Payload = payload;
        SetHttpStatusCode((int)statusCode);
    }
    private void SetHttpStatusCode(int status)
    {
        if (Enumerable.Range(400, 499).Contains(HttpStatus))
        {
            HttpStatus = status;
            return;
        }

        HttpStatus = (int)HttpStatusCode.BadRequest;
    }
    public int HttpStatus { get; private set; }
    public object Payload { get; }
}
