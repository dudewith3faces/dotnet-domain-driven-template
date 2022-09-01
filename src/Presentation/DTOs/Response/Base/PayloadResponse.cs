namespace Presentation.DTOs.Response.Base;

public record PayloadResponse<T> : BaseResponse
{
    public PayloadResponse(T data) : base()
    {
        Data = data;
    }
    public T Data { get; }
}