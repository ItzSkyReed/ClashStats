using System.Text.Json.Serialization;
using Ardalis.SmartEnum;
using Ardalis.SmartEnum.SystemTextJson;

namespace Domain.Constants;

[JsonConverter(typeof(SmartEnumValueConverter<ClanWarLeagueState, string>))]
public sealed class ClanWarLeagueState : SmartEnum<ClanWarLeagueState, string>
{
    public static readonly ClanWarLeagueState GroupNotFound = new(nameof(GroupNotFound), "groupNotFound");
    public static readonly ClanWarLeagueState NotInWar = new(nameof(NotInWar), "notInWar");
    public static readonly ClanWarLeagueState Preparation = new(nameof(Preparation), "preparation");
    public static readonly ClanWarLeagueState InWar = new(nameof(InWar), "inWar");
    public static readonly ClanWarLeagueState WarEnded = new(nameof(WarEnded), "warEnded");

    private ClanWarLeagueState(string name, string value) : base(name, value)
    {
    }
}