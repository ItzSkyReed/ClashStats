using System.ComponentModel;
using Application.Interfaces;
using Domain.Models.Analytics.ClanWarLeagues;
using Microsoft.EntityFrameworkCore;

namespace App.Endpoints;

public static class SummaryEndpoints
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/summaries")
            .WithTags("Аналитическая сводка");
        group.MapGet("/cw/wars", async (IAppDbContext dbContext) =>
            {
                var summaries = await dbContext.ClanWarSummaries.AsNoTracking().ToListAsync();
                return TypedResults.Ok(summaries);
            })
            .WithName("get_clan_war_summaries")
            .WithSummary("Получить общую статистику по войнам")
            .WithDescription("Возвращает агрегированную статистику по каждой прошедшей обычной войне.");

        group.MapGet("/cw/players", async (IAppDbContext dbContext) =>
            {
                var playerSummaries = await dbContext.ClanWarPlayerSummaries.AsNoTracking().ToListAsync();
                return TypedResults.Ok(playerSummaries);
            })
            .WithName("get_clan_war_player_summaries")
            .WithSummary("Получить статистику игроков в КВ")
            .WithDescription("Возвращает накопленную за всё время статистику игроков в обычных клановых войнах.");

        group.MapGet("/players", async (IAppDbContext dbContext) =>
            {
                var playerSummaries = await dbContext.ClanMembers.AsNoTracking()
                    .Include(member => member.SeasonStats)
                    .Include(member => member.ClanWarPlayerSummary)
                    .Include(member => member.ClanWarLeaguesPlayerSummaries)
                    .AsSplitQuery().ToListAsync();
                return TypedResults.Ok(playerSummaries);
            })
            .WithName("get_player_summaries")
            .WithSummary("Получить полную статистику игрока")
            .WithDescription("Возвращает накопленную за всё время статистику игрока");

        group.MapGet("/cwl/players", async Task<IResult> (
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
                        return TypedResults.Ok(Enumerable.Empty<ClanWarLeaguesPlayerSummary>());
                    }
                }

                var summaries = await dbContext.ClanWarLeaguePlayerSummaries
                    .AsNoTracking()
                    .Where(x => x.Season == season)
                    .ToListAsync();

                return TypedResults.Ok(summaries);
            })
            .WithName("get_cwl_player_summaries")
            .WithSummary("Получить статистику игроков в ЛВК за сезон")
            .WithDescription(
                "Возвращает агрегированную статистику игроков за конкретный сезон ЛВК. Если сезон не указан, возвращает данные за самый свежий доступный сезон.");

        group.MapGet("/cwl/groups", async (IAppDbContext dbContext) =>
            {
                var groupSummaries = await dbContext.ClanWarLeagueGroupSummaries
                    .AsNoTracking()
                    .OrderByDescending(x => x.Season)
                    .ToListAsync();

                return TypedResults.Ok(groupSummaries);
            })
            .WithName("get_cwl_group_summaries")
            .WithSummary("Получить историю сезонов ЛВК")
            .WithDescription("Возвращает общую статистику клана по всем историческим сезонам ЛВК (места, победы, деструкшн).");
    }
}