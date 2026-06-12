using Domain.Models.Analytics.ClanWars;
using Riok.Mapperly.Abstractions;
using Shared.DTOs.Summaries;

namespace Application.Mappings;

[Mapper]
public static partial class ClanWarSummaryMapper
{
    [MapperIgnoreSource(nameof(ClanWarSummary.Id))]
    public static partial ClanWarSummaryDto ToDto(this ClanWarSummary clanWarPlayerSummary);

    public static partial IQueryable<ClanWarSummaryDto> ProjectToDto(this IQueryable<ClanWarSummary> query);
}