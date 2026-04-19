using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Dima.Core.Responses.Categories;

namespace Dima.Api.Endpoints.Categories;

public class DeleteCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapDelete("/{id:long}", HandleAsync)
            .WithName("Categories: Delete")
            .WithSummary("Delete an existing category")
            .WithDescription("Deletes an existing category with the provided details.")
            .Produces<BaseResponse<CategoryResponse?>>(200);

    public static async Task<IResult> HandleAsync(
        long id,
        ClaimsPrincipal user,
        ICategoryHandler handler
    )
    {
        var request = new DeleteCategoryRequest
        {
            Id = id,
            UserId =
                user.Identity?.Name
                ?? throw new InvalidOperationException("User ID not found in claims."),
        };
        var result = await handler.DeleteAsync(request);
        return result.ToResult();
    }
}
