using Application.DTOs.Common;
using Application.Interfaces;

namespace Infrastructure.Api;

public record ApiResult<T>(T? Data, ClientErrorDto? Error) : IApiResult<T>
{
    public bool IsSuccess => Error is null;
}