using System.Net.Http.Json;
using Client.ViewModels;
using Domain.Models;
using Microsoft.AspNetCore.Components;

namespace Client.Pages;

public partial class Roster
{
    private List<FullPlayerViewModel> _players = [];

    private string _searchString = "";
    [Inject] private HttpClient Http { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        var members = await Http.GetFromJsonAsync<List<ClanMember>>("/api/v1/summaries/players") ?? [];

        _players = members
            .Select(m => new FullPlayerViewModel(m))
            .OrderByDescending(p => p.Member.TownHallLevel)
            .ThenByDescending(p => p.CurrentSeasonDonations)
            .ToList();
    }


    private static string GetRoleName(string roleName)
    {
        return roleName switch
        {
            "Admin" => "Старейшина",
            "Member" => "Участник",
            "CoLeader" => "Соруководитель",
            "Leader" => "Глава",
            _ => "Роль: Н/Д"
        };
    }

    private bool FilterFunc(FullPlayerViewModel player)
    {
        if (string.IsNullOrWhiteSpace(_searchString)) return true;

        return player.Member.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase)
               || player.Member.Tag.Contains(_searchString, StringComparison.OrdinalIgnoreCase);
    }
}