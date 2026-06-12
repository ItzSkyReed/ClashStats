using System.Net;
using Application.ClashOfClansModels.Common;
using Application.Interfaces;

namespace Infrastructure.Api;

public record ApiResult<T>(T? Data, ClientErrorDto? Error, HttpStatusCode StatusCode) : IApiResult<T>
{
    public bool IsSuccess => Error is null;
}