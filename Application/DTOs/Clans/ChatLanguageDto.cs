namespace Application.DTOs.Clans;

public record ChatLanguageDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string LanguageCode { get; init; }
}