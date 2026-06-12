using System.Text.Json;

namespace Application.ClashOfClansModels.Common;

public record ClientErrorDto
{
    public string? Reason { get; init; }
    public string? Message { get; init; }
    public string? Type { get; init; }
    public Dictionary<JsonElement, JsonElement>? Detail { get; init; }
};