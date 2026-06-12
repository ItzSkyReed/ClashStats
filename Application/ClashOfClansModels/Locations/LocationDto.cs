namespace Application.ClashOfClansModels.Locations;

public record LocationDto
{
    public string? LocalizedName { get; init; }
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required bool IsCountry { get; init; }
    public string? CountryCode { get; init; }
}