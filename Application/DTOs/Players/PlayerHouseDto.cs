namespace Application.DTOs.Players;

public record PlayerHouseDto
{
    public required List<PlayerHouseElementDto> Elements { get; init; }
}