using Application.DTOs.Common;

namespace Application.Interfaces;

public interface IApiResult
{
        bool IsSuccess { get; }
        ClientErrorDto? Error { get; }
}

public interface IApiResult<out T> : IApiResult
{
        T? Data { get; }
}