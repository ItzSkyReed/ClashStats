using System.Text.Json.Serialization;
using Ardalis.SmartEnum;
using Ardalis.SmartEnum.SystemTextJson;

namespace Domain.Constants;

[JsonConverter(typeof(SmartEnumValueConverter<ClanWarLeagueGroupState, string>))]
public sealed class ClanWarLeagueGroupState : SmartEnum<ClanWarLeagueGroupState, string>
{
    public static readonly ClanWarLeagueGroupState GroupNotFound = new(nameof(GroupNotFound), "groupNotFound");
    public static readonly ClanWarLeagueGroupState Preparation = new(nameof(Preparation), "preparation");
    public static readonly ClanWarLeagueGroupState InWar = new(nameof(InWar), "inWar");
    public static readonly ClanWarLeagueGroupState Ended = new(nameof(Ended), "ended");

    private ClanWarLeagueGroupState(string name, string value) : base(name, value)
    {
    }
}