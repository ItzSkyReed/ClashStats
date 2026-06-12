using System.Net;
using Application.ClashOfClansModels.Common;

namespace Application.Interfaces;

public interface IApiResult
{
    bool IsSuccess { get; }
    ClientErrorDto? Error { get; }
    HttpStatusCode StatusCode { get; }
}

public interface IApiResult<out T> : IApiResult
{
    T? Data { get; }
}