namespace Presentation.DTOs.Response.Base;

public record ErrorResponse : BaseResponse
{
    private static string _defaultMessage = "Bad";
    public ErrorResponse(string message) : base(message ?? _defaultMessage)
    {
        Error = new Dictionary<string, string[]>();
    }
    public ErrorResponse(object error) : base(_defaultMessage)
    {
        Error = error;
    }

    public ErrorResponse(string message, object error) : base(message ?? _defaultMessage)
    {
        Error = error ?? new Dictionary<string, string[]>();
    }
    public object Error { get; }
}