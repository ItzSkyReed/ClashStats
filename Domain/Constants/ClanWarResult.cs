using System.Text.Json.Serialization;
using Ardalis.SmartEnum;
using Ardalis.SmartEnum.SystemTextJson;

namespace Domain.Constants;

[JsonConverter(typeof(SmartEnumValueConverter<ClanWarResult, string>))]
public sealed class ClanWarResult : SmartEnum<ClanWarResult, string>
{
    private ClanWarResult(string name, string value) : base(name, value) { }

    public static readonly ClanWarResult Win = new(nameof(Win), "win");
    public static readonly ClanWarResult Lose = new(nameof(Lose), "lose");
    public static readonly ClanWarResult Tie = new(nameof(Tie), "tie");
}