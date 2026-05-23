namespace Application.DTOs.Clans;

public record BadgeUrlsDto
{
    public required string Small { get; init; }
    public required string Large { get; init; }
    public required string Medium { get; init; }
}