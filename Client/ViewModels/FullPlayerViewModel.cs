using Domain.Models;
using Domain.Models.Analytics.ClanWarLeagues;
using Domain.Models.Analytics.ClanWars;
using MudBlazor;

namespace Client.ViewModels;

public class FullPlayerViewModel(ClanMember member)
{
    public ClanMember Member { get; set; } = member;
    public bool ShowDetails { get; set; }

    // --- Главная таблица ---
    public string RoleName => Member.Role?.ToString() ?? "Member";
    public string WarPreferenceIcon => Member.WarPreference?.ToString() == "In" ? Icons.Material.Filled.CheckCircle : Icons.Material.Filled.Cancel;
    public Color WarPreferenceColor => Member.WarPreference?.ToString() == "In" ? Color.Success : Color.Error;

    public int CurrentSeasonDonations => Member.SeasonStats?.OrderByDescending(s => s.SeasonDate).FirstOrDefault()?.Donations ?? 0;

    // --- Явка для главных колонок таблицы ---
    public float CwParticipation => Member.ClanWarPlayerSummary?.AttackParticipationRate ?? 0;
    public float CwlParticipation => LatestCwlSummary?.AttackParticipationRate ?? 0;

    // --- Прямые ссылки на суммари для удобства доступа в UI ---
    public ClanWarPlayerSummary? CwSummary => Member.ClanWarPlayerSummary;

    // ЛВК может быть за несколько сезонов, берем самый актуальный (последний)
    public ClanWarLeaguesPlayerSummary? LatestCwlSummary =>
        Member.ClanWarLeaguesPlayerSummaries?.OrderByDescending(s => s.Season).FirstOrDefault();

    // --- График доната ---
    public double[] DonationHistoryValues => Member.SeasonStats?
        .OrderBy(s => s.SeasonDate)
        .Select(s => (double)s.Donations)
        .ToArray() ?? [];

    public string[] DonationHistoryLabels => Member.SeasonStats?
        .OrderBy(s => s.SeasonDate)
        .Select(s => s.SeasonDate.ToString("MM.yy"))
        .ToArray() ?? [];

    public List<ChartSeries<double>> DonationChartSeries =>
    [
        new ChartSeries<double> { Name = "Отдано", Data = DonationHistoryValues }
    ];
}