namespace Application.ClashOfClansModels.Common;

public record LabelListDto
{
    public required List<LabelDto> Items { get; init; }
    public required PagingDto Paging { get; init; }
}