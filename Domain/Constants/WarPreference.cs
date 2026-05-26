using System.Text.Json.Serialization;
using Ardalis.SmartEnum;
using Ardalis.SmartEnum.SystemTextJson;

namespace Domain.Constants;

[JsonConverter(typeof(SmartEnumValueConverter<WarPreference, string>))]
public sealed class WarPreference : SmartEnum<WarPreference, string>
{
    private WarPreference(string name, string value) : base(name, value) { }

    public static readonly WarPreference In = new(nameof(In), "in");
    public static readonly WarPreference Out = new(nameof(Out), "out");
}