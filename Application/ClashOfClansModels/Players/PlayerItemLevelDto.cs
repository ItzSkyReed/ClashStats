namespace Application.ClashOfClansModels.Players;

public record PlayerItemLevelDto
{
    public required string Name { get; init; }
    public required int Level { get; init; }
    public required int MaxLevel { get; init; }
    public required string Village { get; init; }
    public bool? SuperTroopIsActive { get; init; }
    public List<PlayerHeroEquipmentDto>? Equipment { get; init; }
};