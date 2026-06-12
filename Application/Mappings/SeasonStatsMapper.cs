using Domain.Models;
using Riok.Mapperly.Abstractions;
using Shared.DTOs.Statistics;

namespace Application.Mappings;

[Mapper]
public static partial class SeasonStatsMapper
{
    [MapperIgnoreSource(nameof(SeasonStats.PlayerTag))]
    [MapperIgnoreSource(nameof(SeasonStats.Player))]
    public static partial SeasonStatsDto ToDto(this SeasonStats clanWarPlayerSummary);

    public static partial IQueryable<SeasonStatsDto> ProjectToDto(this IQueryable<SeasonStats> query);
}