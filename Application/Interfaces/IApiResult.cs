using System.Diagnostics.CodeAnalysis;
using System.Net;
using Application.ClashOfClansModels.Common;

namespace Application.Interfaces;

public interface IApiResult
{
    [MemberNotNullWhen(false, nameof(Error))]
    bool IsSuccess { get; }

    ClientErrorDto? Error { get; }
    HttpStatusCode StatusCode { get; }
}

public interface IApiResult<out T> : IApiResult
{
    [MemberNotNullWhen(true, nameof(Data))]
    new bool IsSuccess { get; }

    T? Data { get; }
}