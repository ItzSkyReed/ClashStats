using System.ComponentModel;
using Application.Interfaces;
using Application.Mappings;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Statistics;
using Shared.DTOs.Summaries;

namespace App.Endpoints;

public static class SummaryEndpoints
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        var versionedGroup = app.MapGroup("api/v1");

        versionedGroup.MapGet("/cw/wars/summary", async (IAppDbContext dbContext) =>
            {
                var summaries = await dbContext.ClanWarSummaries.ProjectToDto().ToListAsync();
                return Results.Ok(summaries);
            })
            .WithName("get_clan_war_summaries")
            .WithSummary("Получить общую статистику по войнам")
            .WithDescription("Возвращает агрегированную статистику по каждой прошедшей обычной войне.")
            .Produces<List<ClanWarSummaryDto>>();

        versionedGroup.MapGet("/cw/players/summary", async (IAppDbContext dbContext) =>
            {
                var playerSummaries = await dbContext.ClanWarPlayerSummaries.ProjectToDto().ToListAsync();
                return Results.Ok(playerSummaries);
            })
            .WithName("get_clan_war_player_summaries")
            .WithSummary("Получить статистику игроков в КВ")
            .WithDescription("Возвращает накопленную за всё время статистику игроков в обычных клановых войнах.")
            .Produces<List<ClanWarPlayerSummaryDto>>();

        versionedGroup.MapGet("/players/summary", async (IAppDbContext dbContext) =>
            {
                var playerSummaries = await dbContext.ClanMembers
                    .Include(member => member.SeasonStats)
                    .Include(member => member.ClanWarPlayerSummary)
                    .Include(member => member.ClanWarLeaguesPlayerSummaries)
                    .ProjectToDto()
                    .AsSplitQuery().ToListAsync();
                return Results.Ok(playerSummaries);
            })
            .WithName("get_player_summaries")
            .WithSummary("Получить полную статистику игрока")
            .WithDescription("Возвращает накопленную за всё время статистику игрока")
            .Produces<List<ClanMemberSummaryDto>>();

        versionedGroup.MapGet("/cwl/players/summary", async Task<IResult> (
                [Description("Сезон в формате ГГГГ-ММ (например, 2026-05). Необязательный.")]
                string? season,
                IAppDbContext dbContext) =>
            {
                if (string.IsNullOrWhiteSpace(season))
                {
                    season = await dbContext.ClanWarLeaguePlayerSummaries
                        .AsNoTracking()
                        .Select(x => x.Season)
                        .OrderByDescending(s => s)
                        .FirstOrDefaultAsync();

                    if (season == null)
                    {
                        return Results.Ok(Enumerable.Empty<ClanWarLeaguesPlayerSummaryDto>());
                    }
                }

                var summaries = await dbContext.ClanWarLeaguePlayerSummaries
                    .Where(x => x.Season == season)
                    .ProjectToDto()
                    .ToListAsync();

                return Results.Ok(summaries);
            })
            .WithName("get_cwl_player_summaries")
            .WithSummary("Получить статистику игроков в ЛВК за сезон")
            .WithDescription(
                "Возвращает агрегированную статистику игроков за конкретный сезон ЛВК. Если сезон не указан, возвращает данные за самый свежий доступный сезон.")
            .Produces<List<ClanWarLeaguesPlayerSummaryDto>>();

        versionedGroup.MapGet("/cwl/groups/summary", async (IAppDbContext dbContext) =>
            {
                var groupSummaries = await dbContext.ClanWarLeagueGroupSummaries
                    .OrderByDescending(x => x.Season)
                    .ProjectToDto()
                    .ToListAsync();

                return Results.Ok(groupSummaries);
            })
            .WithName("get_cwl_group_summaries")
            .WithSummary("Получить историю сезонов ЛВК")
            .WithDescription("Возвращает общую статистику клана по всем историческим сезонам ЛВК (места, победы, разрушений).")
            .Produces<List<ClanWarLeagueGroupSummaryDto>>();

        versionedGroup.MapGet("/player/activity-chart/{tag}", async (
                string tag,
                DateTimeOffset? start,
                DateTimeOffset? end,
                IPlayerActivityService activityService) =>
            {
                var actualEnd = end ?? DateTimeOffset.UtcNow;
                var actualStart = start ?? actualEnd.AddDays(-30);

                var chartData = await activityService.GetNormalizedMemberChartAsync(tag, actualStart, actualEnd);

                return Results.Ok(chartData);
            })
            .WithName("get_player_activity_chart")
            .WithSummary("Получить график активности игрока")
            .WithDescription("Возвращает график активности игрока за определенный период.")
            .Produces<List<ChartPointDto>>();
    }
}