using Domain.Models.Analytics.ClanWarLeagues;
using Domain.Models.Analytics.ClanWars;

namespace Client.ViewModels;

public class PlayerStatsViewModel
{
    public string Tag { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool ShowDetails { get; set; }

    // Ссылки на оригинальные модели (могут быть null, если игрок не участвовал)
    public ClanWarPlayerSummary? CwStats { get; set; }
    public ClanWarLeaguesPlayerSummary? CwlStats { get; set; }

    // Удобные проперти для поиска и вывода в главной строке
    public float CwParticipation => CwStats?.AttackParticipationRate ?? 0;
    public float CwlParticipation => CwlStats?.AttackParticipationRate ?? 0;
}