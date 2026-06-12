using Domain.Models;
using Riok.Mapperly.Abstractions;
using Shared.DTOs.Summaries;

namespace Application.Mappings;

[Mapper,
 UseStaticMapper(typeof(SeasonStatsMapper)),
 UseStaticMapper(typeof(ClanWarLeaguesPlayerSummaryMapper)),
 UseStaticMapper(typeof(ClanWarPlayerSummaryMapper))]
public static partial class ClanMemberMapper
{
    [MapperIgnoreSource(nameof(ClanMember.InternalId)),
     MapperIgnoreSource(nameof(ClanMember.ClanWarPerformances)),
     MapperIgnoreSource(nameof(ClanMember.ClanWarLeaguePerformances)),
     MapperIgnoreSource(nameof(ClanMember.ActivitySnapshots)),
     MapperIgnoreSource(nameof(ClanMember.ActivityState))]
    public static partial ClanMemberSummaryDto ToDto(this ClanMember clanMember);

    public static partial IQueryable<ClanMemberSummaryDto> ProjectToDto(this IQueryable<ClanMember> query);
}