using Application.Interfaces;

namespace App.Workers;

public partial class StatsUpdateWorker(ILogger<StatsUpdateWorker> logger, IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        logger.LogInformation("Worker loaded. Initializing cache and data seed...");

        try
        {
            using var scope = serviceProvider.CreateScope();
            var initializer = scope.ServiceProvider.GetRequiredService<IClanDataSyncService>();
            logger.LogInformation("Seeding initial clan members...");
            await initializer.UpdateClanMembers(ct);

            logger.LogInformation("Seeding initial clan war data...");
            await initializer.UpdateClanWar(ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to run initial data seed. Moving to timers...");
        }

        var clanMembersTask = RunTaskWithTimerAsync(
            TimeSpan.FromMinutes(5),
            collector => collector.UpdateClanMembers(ct),
            "ClanMembers", ct);

        var clanWarTask = RunTaskWithTimerAsync(
            TimeSpan.FromMinutes(1),
            async collector =>
            {
                var hasChanges = await collector.UpdateClanWar(ct);
                if (hasChanges)
                {
                    await collector.RefreshMaterializedViews(ct);
                }
            },
            "ClanWarAndViews", ct);

        var seasonStatsTask = RunTaskWithTimerAsync(
            TimeSpan.FromMinutes(10),
            collector => collector.UpdateSeasonStats(ct),
            "SeasonStats", ct);

        var clanLeagueWarsTask = RunTaskWithTimerAsync(
            TimeSpan.FromMinutes(1),
            collector => collector.UpdateClanLeagueWars(ct),
            "LeagueWars", ct);

        var cleanupStuckWarsTask = RunTaskWithTimerAsync(
            TimeSpan.FromMinutes(10),
            collector => collector.CleanupStuckWars(ct),
            "StuckWars", ct);

        await Task.WhenAll(clanMembersTask, seasonStatsTask, clanWarTask, cleanupStuckWarsTask, clanLeagueWarsTask);
    }

    private async Task RunTaskWithTimerAsync(TimeSpan period, Func<IClanDataSyncService, Task> action, string taskName, CancellationToken ct)
    {
        using var timer = new PeriodicTimer(period);
        try
        {
            while (await timer.WaitForNextTickAsync(ct))
            {
                try
                {
                    LogExecutingBackgroundTaskTaskName(taskName);
                    using var scope = serviceProvider.CreateScope();
                    var collector = scope.ServiceProvider.GetRequiredService<IClanDataSyncService>();

                    await action(collector);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error executing task: {TaskName}", taskName);
                }
            }
        }
        catch (OperationCanceledException)
        {
            LogStoppingTaskTaskName(taskName);
        }
    }

    [LoggerMessage(LogLevel.Information, "Executing background task: {TaskName}")]
    partial void LogExecutingBackgroundTaskTaskName(string taskName);

    [LoggerMessage(LogLevel.Information, "Stopping task {TaskName}...")]
    partial void LogStoppingTaskTaskName(string taskName);
}