using Dima.Core.Responses;

namespace Dima.Api.Common.Api;

public static class ResultExtensions
{
    public static IResult ToResult<T>(this BaseResponse<T> response, string? createdAtUri = null)
    {
        return response.Code switch
        {
            201 => TypedResults.Created(createdAtUri, response),
            204 => TypedResults.NoContent(),
            400 => TypedResults.BadRequest(response),
            401 => TypedResults.Unauthorized(),
            403 => TypedResults.Forbid(),
            404 => TypedResults.NotFound(response),
            _ when response.IsSuccess => TypedResults.Ok(response),
            _ => TypedResults.Problem(response.Message),
        };
    }
}
