namespace Presentation.Options;

public struct SwaggerOption
{
    public bool ShowSwagger { get; init; }
    public string? Title { get; init; }
    public string? Endpoint { get; init; }
    public string[]? Versions { get; init; }
}