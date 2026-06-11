using System.Text.Json.Serialization;

namespace Domain.Models;

public class PlayerActivityState
{
    public int MemberInternalId { get; set; }

    /// <summary>
    /// Аккумулирует сумму столбцов, сравнивается для добавления нового времени захода
    /// </summary>
    public long ActivityScore { get; set; }

    [JsonIgnore] public ClanMember? Member { get; set; }
}