using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using Dima.Core.Responses.Transactions;

namespace Dima.Api.Endpoints.Transactions;

public class UpdateTransactionEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapPut("/{id:long}", HandleAsync)
            .WithName("Transactions: Update")
            .WithSummary("Update an existing transaction")
            .WithDescription("Updates an existing transaction with the provided details.")
            .Produces<BaseResponse<TransactionResponse?>>(200);

    public static async Task<IResult> HandleAsync(
        long id,
        ClaimsPrincipal user,
        UpdateTransactionRequest request,
        ITransactionHandler handler
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
