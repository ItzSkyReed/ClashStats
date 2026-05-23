using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.Api.Converters;

public class ClashDateTimeConverter : JsonConverter<DateTime>
{
    private const string DateFormat = "yyyyMMdd'T'HHmmss.fff'Z'";

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var s = reader.GetString();
        return DateTime.TryParseExact(s, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out var date)
            ? date
            : throw new JsonException($"Unable to parse date: {s}");
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(DateFormat, CultureInfo.InvariantCulture));
    }
}