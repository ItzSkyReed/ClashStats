using System.Text.Json.Serialization;
using Ardalis.SmartEnum;
using Ardalis.SmartEnum.SystemTextJson;

namespace Domain.Constants;

[JsonConverter(typeof(SmartEnumValueConverter<ClanRole, string>))]
public sealed class ClanRole : SmartEnum<ClanRole, string>
{
    public static readonly ClanRole NotMember = new(nameof(NotMember), "notMember");
    public static readonly ClanRole Member = new(nameof(Member), "member");
    public static readonly ClanRole Leader = new(nameof(Leader), "leader");
    public static readonly ClanRole CoLeader = new(nameof(CoLeader), "coLeader");
    public static readonly ClanRole Admin = new(nameof(Admin), "admin");

    private ClanRole(string name, string value) : base(name, value)
    {
    }
}