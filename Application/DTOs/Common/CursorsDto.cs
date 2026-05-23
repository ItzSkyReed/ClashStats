namespace Application.DTOs.Common;

public record CursorsDto
{
    public string? After { get; init; }
    public string? Before { get; init; }
}