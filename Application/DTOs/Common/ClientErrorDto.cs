using System.Text.Json;

namespace Application.DTOs.Common;

public record ClientErrorDto
{
    public required string Reason{ get; init; }
    public required string Message{ get; init; }
    public required string Type{ get; init; }
    public required Dictionary<JsonElement, JsonElement> Detail{ get; init; }
};