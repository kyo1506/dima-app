using System.Security.Claims;
using Dima.Api.Common.Api;

namespace Dima.Api.Endpoints.Identity;

public class GetRolesEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapGet("/roles", Handle)
            .WithName("Get Roles")
            .WithSummary("Retrieves the roles of the current user")
            .WithDescription(
                "This endpoint retrieves the roles of the currently authenticated user. It requires the user to be authenticated before they can access this endpoint. Upon successful retrieval, it returns a list of roles associated with the user."
            )
            .RequireAuthorization();

    public static IResult Handle(ClaimsPrincipal user)
    {
        if (user.Identity is null || !user.Identity.IsAuthenticated)
            return Results.Unauthorized();

        var identity = (ClaimsIdentity)user.Identity;

        var roles = identity
            .FindAll(identity.RoleClaimType)
            .Select(c => new
            {
                c.Issuer,
                c.OriginalIssuer,
                c.Type,
                c.Value,
                c.ValueType,
            })
            .ToList();

        return TypedResults.Json(roles);
    }
}
