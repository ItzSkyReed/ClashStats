namespace Application.DTOs.Common;

public record PagingDto
{
    public required CursorsDto Cursors { get; init; }
}