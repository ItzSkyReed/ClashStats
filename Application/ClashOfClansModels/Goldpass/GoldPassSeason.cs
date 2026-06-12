namespace Application.ClashOfClansModels.Goldpass;

public record GoldPassSeason
{
    public required DateTime StartTime { get; init; }
    public required DateTime EndTime { get; init; }
}