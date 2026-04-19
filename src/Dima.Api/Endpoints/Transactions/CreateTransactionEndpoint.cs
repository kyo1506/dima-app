using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using Dima.Core.Responses.Transactions;

namespace Dima.Api.Endpoints.Transactions;

public class CreateTransactionEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapPost("/", HandleAsync)
            .WithName("Transactions: Create")
            .WithSummary("Create a new transaction")
            .WithDescription("Creates a new transaction with the provided details.")
            .Produces<BaseResponse<TransactionResponse?>>(201);

    public static async Task<IResult> HandleAsync(
        CreateTransactionRequest request,
        ClaimsPrincipal user,
        ITransactionHandler handler
    )
    {
        request.UserId =
            user.Identity?.Name
            ?? throw new InvalidOperationException("User ID not found in claims.");
        var result = await handler.CreateAsync(request);
        return result.ToResult($"/v1/transactions/{result.Data?.Id}");
    }
}
