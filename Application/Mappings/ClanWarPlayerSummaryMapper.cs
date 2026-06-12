using Domain.Models.Analytics.ClanWars;
using Riok.Mapperly.Abstractions;
using Shared.DTOs.Summaries;

namespace Application.Mappings;

[Mapper]
public static partial class ClanWarPlayerSummaryMapper
{
    [MapperIgnoreSource(nameof(ClanWarPlayerSummary.ClanMember))]
    public static partial ClanWarPlayerSummaryDto ToDto(this ClanWarPlayerSummary clanWarPlayerSummary);

    public static partial IQueryable<ClanWarPlayerSummaryDto> ProjectToDto(this IQueryable<ClanWarPlayerSummary> query);
}