using Application.DTOs.Common;

namespace Application.DTOs.Locations;

public record LocationListDto
{
    public required List<LocationDto> Items { get; init; }
    public required PagingDto PageInfo { get; init; }
}