using System.Text.Json.Serialization;

namespace Dima.Core.Responses;

public class BaseResponse<T>
{
    public BaseResponse()
    {
        _code = Configuration.DefaultSuccessCode;
    }

    [JsonConstructor]
    public BaseResponse(
        T? data,
        int code = Configuration.DefaultSuccessCode,
        string? message = null
    )
    {
        _code = code;
        Data = data;
        Message = message;
    }

    private readonly int _code;
    public int Code => _code;
    public bool IsSuccess => _code >= 200 && _code < 300;
    public T? Data { get; set; }
    public string? Message { get; set; }
}
