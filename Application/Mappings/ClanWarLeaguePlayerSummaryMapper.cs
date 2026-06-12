using Domain.Models.Analytics.ClanWarLeagues;
using Riok.Mapperly.Abstractions;
using Shared.DTOs.Summaries;

namespace Application.Mappings;

[Mapper]
public static partial class ClanWarLeaguesPlayerSummaryMapper
{
    [MapperIgnoreSource(nameof(ClanWarLeaguesPlayerSummary.ClanMember))]
    public static partial ClanWarLeaguesPlayerSummaryDto ToDto(this ClanWarLeaguesPlayerSummary clanWarLeaguesPlayerSummary);

    public static partial IQueryable<ClanWarLeaguesPlayerSummaryDto> ProjectToDto(this IQueryable<ClanWarLeaguesPlayerSummary> query);
}