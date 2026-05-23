using Ardalis.SmartEnum;

namespace Domain.Constants;

public sealed class WarFrequency : SmartEnum<WarFrequency, string>
{
    // Конструктор: передаем имя (для C#) и значение (строку из API)
    private WarFrequency(string name, string value) : base(name, value) { }

    public static readonly WarFrequency Unknown = new(nameof(Unknown), "unknown");
    public static readonly WarFrequency Always = new(nameof(Always), "always");
    public static readonly WarFrequency MoreThanOncePerWeek = new(nameof(MoreThanOncePerWeek), "more_than_once_per_week");
    public static readonly WarFrequency OncePerWeek = new(nameof(OncePerWeek), "once_per_week");
    public static readonly WarFrequency LessThanOncePerWeek = new(nameof(LessThanOncePerWeek), "less_than_once_per_week");
    public static readonly WarFrequency Never = new(nameof(Never), "never");
    public static readonly WarFrequency Any = new(nameof(Any), "any");
}