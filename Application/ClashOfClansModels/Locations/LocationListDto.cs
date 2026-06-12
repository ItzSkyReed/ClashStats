using Application.ClashOfClansModels.Common;

namespace Application.ClashOfClansModels.Locations;

public record LocationListDto
{
    public required List<LocationDto> Items { get; init; }
    public required PagingDto PageInfo { get; init; }
}