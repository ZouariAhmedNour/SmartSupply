namespace SmartSupply.Application.Common
{
    public record Result<T>(bool IsSuccess, T? Data = default, string? Error = null);

    public record Result(bool IsSuccess, object? Data = null, string? Error = null)
        : Result<object?>(IsSuccess, Data, Error);
}