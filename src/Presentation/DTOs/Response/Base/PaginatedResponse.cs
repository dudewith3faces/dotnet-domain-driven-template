namespace Presentation.DTOs.Response.Base;

public record PaginatedResponse<T> : PayloadResponse<IEnumerable<T>>
{
    public PaginatedResponse(IEnumerable<T> payload, int currentPage, int pageSize) : base(payload)
    {
        CurrentPage = currentPage;
        HasPrevious = CurrentPage > 1;
        HasNext = payload.Count<T>() == pageSize;
    }
    public bool HasNext { get; }
    public bool HasPrevious { get; }
    public int CurrentPage { get; }
}