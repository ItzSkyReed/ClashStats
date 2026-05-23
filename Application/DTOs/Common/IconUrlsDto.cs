namespace Application.DTOs.Common;

public record IconUrlsDto
{
    public string? Small { get; init; }
    public string? Tiny { get; init; }
    public string? Medium { get; init; }
    public string? Large { get; init; }
}