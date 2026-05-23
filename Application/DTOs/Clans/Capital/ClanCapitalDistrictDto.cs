namespace Application.DTOs.Clans.Capital;

public record ClanCapitalDistrictDto
{
    public required int Id  { get; init; }
    public required string Name { get; init; }
    public required int DistrictHallLevel { get; init; }
}