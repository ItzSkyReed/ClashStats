namespace Application.ClashOfClansModels.Common;

public record LabelDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required IconUrlsDto IconUrls { get; init; }
}