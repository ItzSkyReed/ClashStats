namespace Application.ClashOfClansModels.Common;

public record PagingDto
{
    public required CursorsDto Cursors { get; init; }
}