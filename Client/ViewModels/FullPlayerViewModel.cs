using MudBlazor;
using Shared.DTOs.Summaries;

namespace Client.ViewModels;

public class FullPlayerViewModel(ClanMemberSummaryDto member)
{
    public ClanMemberSummaryDto Member { get; set; } = member;
    public bool ShowDetails { get; set; }

    public string RoleName => Member.Role ?? "notMember";
    public string WarPreferenceIcon => Member.WarPreference == "in" ? Icons.Material.Filled.CheckCircle : Icons.Material.Filled.Cancel;
    public Color WarPreferenceColor => Member.WarPreference == "in" ? Color.Success : Color.Error;

    public int CurrentSeasonDonations => Member.SeasonStats.OrderByDescending(s => s.SeasonDate).FirstOrDefault()?.Donations ?? 0;

    public float CwParticipation => Member.ClanWarPlayerSummary?.AttackParticipationRate ?? 0;
    public float CwlParticipation => LatestCwlSummary?.AttackParticipationRate ?? 0;

    public ClanWarPlayerSummaryDto? CwSummary => Member.ClanWarPlayerSummary;

    public ClanWarLeaguesPlayerSummaryDto? LatestCwlSummary =>
        Member.ClanWarLeaguesPlayerSummaries?.OrderByDescending(s => s.Season).FirstOrDefault();

    public double[] DonationHistoryValues => Member.SeasonStats
        .OrderBy(s => s.SeasonDate)
        .Select(s => (double)s.Donations)
        .ToArray();

    public string[] DonationHistoryLabels => Member.SeasonStats
        .OrderBy(s => s.SeasonDate)
        .Select(s => s.SeasonDate.ToString("MM.yy"))
        .ToArray();

    public List<ChartSeries<double>> DonationChartSeries =>
    [
        new() { Name = "Отдано", Data = DonationHistoryValues }
    ];
}