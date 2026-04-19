using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using Dima.Core.Responses.Transactions;

namespace Dima.Api.Endpoints.Transactions;

public class GetTransactionByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapGet("/{id:long}", HandleAsync)
            .WithName("Transactions: Get By Id")
            .WithSummary("Get a transaction by Id")
            .WithDescription("Retrieves a transaction by its Id.")
            .Produces<BaseResponse<TransactionResponse?>>(200);

    public static async Task<IResult> HandleAsync(
        long id,
        ClaimsPrincipal user,
        ITransactionHandler handler
    )
    {
        var request = new GetTransactionByIdRequest
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
