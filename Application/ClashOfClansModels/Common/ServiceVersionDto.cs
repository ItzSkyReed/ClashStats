namespace Application.ClashOfClansModels.Common;

public record ServiceVersionDto
{
    public required int Major { get; init; }
    public required int Minor { get; init; }
    public required int Content { get; init; }
}