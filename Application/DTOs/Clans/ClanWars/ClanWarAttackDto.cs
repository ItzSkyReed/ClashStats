namespace Application.DTOs.Clans.ClanWars;

public record ClanWarAttackDto
{
    public required string AttackerTag{ get; init; }
    public required string DefenderTag{ get; init; }
    public required int Stars{ get; init; }
    public required int DestructionPercentage{ get; init; }
    public required int Order{ get; init; }
    public required int Duration{ get; init; }
}