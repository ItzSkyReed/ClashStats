namespace Application.ClashOfClansModels.Players;

public record PlayerAchievementProgressDto
{
    public required int Stars { get; init; }
    public required int Value { get; init; }
    public required string Name { get; init; }
    public required int Target { get; init; }
    public required string Info { get; init; }
    public required string CompletionInfo { get; init; }
    public required string Village { get; init; }
};