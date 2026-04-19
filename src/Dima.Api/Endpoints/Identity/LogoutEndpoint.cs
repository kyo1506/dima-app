using Dima.Api.Common.Api;
using Dima.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace Dima.Api.Endpoints.Identity;

public class LogoutEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapPost("/logout", HandleAsync)
            .WithName("Logout")
            .WithSummary("Logs out the current user")
            .WithDescription(
                "This endpoint logs out the currently authenticated user by signing them out of the application. It requires the user to be authenticated before they can access this endpoint. Upon successful logout, it returns a confirmation message."
            )
            .RequireAuthorization();

    public static async Task<IResult> HandleAsync(SignInManager<User> signInManager)
    {
        await signInManager.SignOutAsync();
        return Results.Ok(new { message = "Logged out successfully." });
    }
}
