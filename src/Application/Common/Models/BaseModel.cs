namespace Application.Common.Models;

public abstract record BaseModel
{
    protected BaseModel()
    { }

    public virtual long Id { get; init; } = default;
    public DateTime DateCreated { get; init; }
}