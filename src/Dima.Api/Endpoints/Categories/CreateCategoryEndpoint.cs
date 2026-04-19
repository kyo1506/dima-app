using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Dima.Core.Responses.Categories;

namespace Dima.Api.Endpoints.Categories;

public class CreateCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapPost("/", HandleAsync)
            .WithName("Categories: Create")
            .WithSummary("Create a new category")
            .WithDescription("Creates a new category with the provided details.")
            .Produces<BaseResponse<CategoryResponse?>>(201);

    public static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        CreateCategoryRequest request,
        ICategoryHandler handler
    )
    {
        request.UserId =
            user.Identity?.Name
            ?? throw new InvalidOperationException("User ID not found in claims.");
        var result = await handler.CreateAsync(request);
        return result.ToResult($"/v1/categories/{result.Data?.Id}");
    }
}
