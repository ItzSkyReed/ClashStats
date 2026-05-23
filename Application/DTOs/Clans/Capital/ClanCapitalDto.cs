namespace Application.DTOs.Clans.Capital;

public record ClanCapitalDto
{
    public required int CapitalHallLevel { get; init; }
    public required List<ClanCapitalDistrictDto> Districts { get; init; }
}