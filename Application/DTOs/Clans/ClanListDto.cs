using Application.DTOs.Common;

namespace Application.DTOs.Clans;

public record ClanListDto
{
    public required List<ClanDto> Items { get; init; }
    public PagingDto? Paging { get; init; }
};