using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using Dima.Core.Responses.Transactions;

namespace Dima.Api.Endpoints.Transactions;

public class DeleteTransactionEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapDelete("/{id:long}", HandleAsync)
            .WithName("Transactions: Delete")
            .WithSummary("Delete an existing transaction")
            .WithDescription("Deletes an existing transaction with the provided details.")
            .Produces<BaseResponse<TransactionResponse?>>(200);

    public static async Task<IResult> HandleAsync(
        long id,
        ClaimsPrincipal user,
        ITransactionHandler handler
    )
    {
        var request = new DeleteTransactionRequest
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
