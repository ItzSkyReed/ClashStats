using System.Net.Http.Json;
using Client.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shared.DTOs.Statistics;

namespace Client.Components;

partial class PlayerDetailsCard
{
    private string[] _activityLabels = [];
    private bool _activityLoaded;
    private List<ChartSeries<double>> _activitySeries = [];

    private string _activitySubtitle = "Активность";

    private bool _isLoadingActivity;
    [Inject] private HttpClient Http { get; set; } = null!;

    [Parameter] public FullPlayerViewModel Player { get; set; } = default!;

    private async Task OnTabChanged(int tabIndex)
    {
        if (tabIndex == 3 && !_activityLoaded)
        {
            await LoadActivityDataAsync();
        }
    }

    private async Task LoadActivityDataAsync()
    {
        try
        {
            _isLoadingActivity = true;

            var safeTag = Uri.EscapeDataString(Player.Member.Tag);

            var data = await Http.GetFromJsonAsync<List<ChartPointDto>>($"api/v1/player/activity-chart/{safeTag}");

            if (data is not null && data.Count != 0)
            {
                var earliestDate = data.Min(x => x.Time);
                var latestDate = data.Max(x => x.Time);
                var totalDays = (int)Math.Ceiling((latestDate - earliestDate).TotalDays);

                _activitySubtitle = totalDays switch
                {
                    < 1 => $"Примерный онлайн за {earliestDate:dd.MM.yyyy}",
                    _ => $"Примерный онлайн за {totalDays} дн. ({earliestDate:dd.MM} — {latestDate:dd.MM})"
                };

                _activitySeries =
                [
                    new ChartSeries<double>
                    {
                        Name = "Онлайн (минут)",

                        Data = new ChartData<double>(data.Select(x => (double)(x.ActivityScore * 10)).ToArray())
                    }
                ];

                var rawLabels = totalDays <= 2
                    ? data.Select(x => x.Time.ToString("HH:mm")).ToArray()
                    : data.Select(x => x.Time.ToString("dd.MM")).ToArray();

                var step = rawLabels.Length > 10 ? (int)Math.Ceiling(rawLabels.Length / 10.0) : 1;

                _activityLabels = rawLabels
                    .Select((label, index) => index % step == 0 ? label : "")
                    .ToArray();
            }

            _activityLoaded = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка загрузки графика активности: {ex.Message}");
        }
        finally
        {
            _isLoadingActivity = false;
        }
    }

    private static Color GetCwAttackColor(short attacks) => attacks switch
    {
        2 => Color.Success,
        1 => Color.Warning,
        0 => Color.Error,
        _ => Color.Default
    };

    private static Color GetCwlRoundColor(short result) => result switch
    {
        3 => Color.Success,
        2 or 1 => Color.Warning,
        0 => Color.Error,
        -1 => Color.Info,
        _ => Color.Surface
    };

    private static string GetCwlRoundText(short result) => result switch
    {
        >= 0 => result.ToString(),
        -1 => "Z",
        _ => "-"
    };
}