using Ardalis.SmartEnum;

namespace Domain.Constants;

public sealed class ClanWarState : SmartEnum<ClanWarState, string>
{
    private ClanWarState(string name, string value) : base(name, value)
    {
    }

    public static readonly ClanWarState ClanNotFound = new(nameof(ClanNotFound), "clan_not_found");
    public static readonly ClanWarState AccessDenied = new(nameof(AccessDenied), "access_denied");
    public static readonly ClanWarState NotInWar = new(nameof(NotInWar), "notInWar");

    [Obsolete("Not used now as far as I understand")]
    public static readonly ClanWarState InMatchmaking = new(nameof(InMatchmaking), "inMatchmaking");

    [Obsolete("Not used now as far as I understand")]
    public static readonly ClanWarState EnterWar = new(nameof(EnterWar), "enterWar");

    [Obsolete("Not used now as far as I understand")]
    public static readonly ClanWarState Matched = new(nameof(Matched), "matched");

    public static readonly ClanWarState Preparation = new(nameof(Preparation), "preparation");

    [Obsolete("Not used now as far as I understand")]
    public static readonly ClanWarState War = new(nameof(War), "war");

    public static readonly ClanWarState InWar = new(nameof(InWar), "inWar");
    public static readonly ClanWarState Ended = new(nameof(Ended), "ended");
}