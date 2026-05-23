using Ardalis.SmartEnum;

namespace Domain.Constants;

public sealed class ClanRole : SmartEnum<ClanRole, string>
{
    private ClanRole(string name, string value) : base(name, value) { }

    public static readonly ClanRole NotMember = new(nameof(NotMember), "not_member");
    public static readonly ClanRole Member = new(nameof(Member), "member");
    public static readonly ClanRole Leader = new(nameof(Leader), "leader");
    public static readonly ClanRole CoLeader = new(nameof(CoLeader), "coLeader");
    public static readonly ClanRole Admin = new(nameof(Admin), "admin");
}