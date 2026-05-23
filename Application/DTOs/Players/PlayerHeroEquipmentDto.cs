namespace Application.DTOs.Players;

public record PlayerHeroEquipmentDto
{
    public required string Name { get; init; }
    public required int Level { get; init; }
    public required int MaxLevel { get; init; }
    public required string Village { get; init; }
}