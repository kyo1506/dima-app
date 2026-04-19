using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Dima.Core.Responses.Categories;

namespace Dima.Api.Endpoints.Categories;

public class GetCategoryByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapGet("/{id:long}", HandleAsync)
            .WithName("Categories: Get By Id")
            .WithSummary("Get a category by Id")
            .WithDescription("Retrieves a category by its Id.")
            .Produces<BaseResponse<CategoryResponse?>>(200);

    public static async Task<IResult> HandleAsync(
        long id,
        ClaimsPrincipal user,
        ICategoryHandler handler
    )
    {
        var request = new GetCategoryByIdRequest
        {
            Id = id,
            UserId =
                user.Identity?.Name
                ?? throw new InvalidOperationException("User ID not found in claims."),
        };

        var result = await handler.GetByIdAsync(request);

        return result.ToResult();
    }
}
