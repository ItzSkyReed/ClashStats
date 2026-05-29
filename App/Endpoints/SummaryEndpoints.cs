using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace App.Endpoints;

public static class SummaryEndpoints
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/summaries")
            .WithTags("Аналитическая сводка");

        group.MapGet("/players", async (IAppDbContext dbContext) =>
            {
                var summaries = await dbContext.ClanWarPlayerSummaries.AsNoTracking().ToListAsync();
                return TypedResults.Ok(summaries);
            })
            .WithName("get_player_summaries")
            .WithSummary("Получить статистику по игрокам")
            .WithDescription("Возвращает агрегированную статистику атак игроков в КВ.");

        group.MapGet("/wars", async (IAppDbContext dbContext) =>
            {
                var summaries = await dbContext.ClanWarSummaries.AsNoTracking().ToListAsync();
                return TypedResults.Ok(summaries);
            })
            .WithName("get_clan_war_summaries")
            .WithSummary("Получить статистику по войнам")
            .WithDescription("Возвращает агрегированную статистику по войнам.");
    }
}