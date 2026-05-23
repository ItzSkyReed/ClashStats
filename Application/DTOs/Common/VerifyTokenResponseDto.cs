namespace Application.DTOs.Common;

public record VerifyTokenResponseDto
{
    public required string Tag { get; init; }
    public required string Token { get; init; }
    public required string Status { get; init; }
};