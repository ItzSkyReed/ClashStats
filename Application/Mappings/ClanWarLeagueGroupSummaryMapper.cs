using Domain.Models.Analytics.ClanWarLeagues;
using Riok.Mapperly.Abstractions;
using Shared.DTOs.Summaries;

namespace Application.Mappings;

[Mapper]
public static partial class ClanWarLeagueGroupSummaryMapper
{
    public static partial ClanWarLeagueGroupSummaryDto ToDto(this ClanWarLeagueGroupSummary clanWarLeagueGroupSummary);

    public static partial IQueryable<ClanWarLeagueGroupSummaryDto> ProjectToDto(this IQueryable<ClanWarLeagueGroupSummary> query);
}