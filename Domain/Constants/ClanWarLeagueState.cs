using System.Text.Json.Serialization;
using Ardalis.SmartEnum;
using Ardalis.SmartEnum.SystemTextJson;

namespace Domain.Constants;

[JsonConverter(typeof(SmartEnumValueConverter<ClanWarLeagueState, string>))]
public sealed class ClanWarLeagueState : SmartEnum<ClanWarLeagueState, string>
{
    private ClanWarLeagueState(string name, string value) : base(name, value) { }

    public static readonly ClanWarLeagueState GroupNotFound = new(nameof(GroupNotFound), "group_not_found");
    public static readonly ClanWarLeagueState NotInWar = new(nameof(NotInWar),"not_in_war");
    public static readonly ClanWarLeagueState Preparation = new(nameof(Preparation),"preparation");
    public static readonly ClanWarLeagueState War = new(nameof(War),"war");
    public static readonly ClanWarLeagueState Ended = new(nameof(Ended),"ended");
}