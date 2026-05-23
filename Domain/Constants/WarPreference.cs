using Ardalis.SmartEnum;

namespace Domain.Constants;

public sealed class WarPreference : SmartEnum<WarPreference, string>
{
    private WarPreference(string name, string value) : base(name, value) { }

    public static readonly WarPreference In = new(nameof(In), "in");
    public static readonly WarPreference Out = new(nameof(Out), "out");
}