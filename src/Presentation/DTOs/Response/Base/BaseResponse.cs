namespace Presentation.DTOs.Response.Base;

public record BaseResponse
{
    private string _defaultMessage = "Ok";
    public BaseResponse()
    {
        Message = _defaultMessage;
    }
    public BaseResponse(string message)
    {
        Message = message ?? _defaultMessage;
    }
    public string Message { get; }
}