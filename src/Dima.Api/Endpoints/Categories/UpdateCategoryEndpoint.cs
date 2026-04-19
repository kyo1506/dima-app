using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Dima.Core.Responses.Categories;

namespace Dima.Api.Endpoints.Categories;

public class UpdateCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapPut("/{id:long}", HandleAsync)
            .WithName("Categories: Update")
            .WithSummary("Update an existing category")
            .WithDescription("Updates an existing category with the provided details.")
            .Produces<BaseResponse<CategoryResponse?>>(200);

    public static async Task<IResult> HandleAsync(
        long id,
        UpdateCategoryRequest request,
        ClaimsPrincipal user,
        ICategoryHandler handler
    )
    {
        request.Id = id;
        request.UserId =
            user.Identity?.Name
            ?? throw new InvalidOperationException("User ID not found in claims.");
        var result = await handler.UpdateAsync(request);
        return result.ToResult();
    }
}
