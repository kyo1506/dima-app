using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core;
using Dima.Core.Handlers;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using Dima.Core.Responses.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace Dima.Api.Endpoints.Transactions;

public class GetTransactionsByPeriodEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapGet("/", HandleAsync)
            .WithName("Transactions: Get By Period")
            .WithSummary("Get all transactions by period with pagination")
            .WithDescription("Retrieves all transactions by period with pagination.")
            .Produces<PagedResponse<List<TransactionResponse>>>();

    public static async Task<IResult> HandleAsync(
        ITransactionHandler handler,
        ClaimsPrincipal user,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] int pageNumber = Configuration.DefaultPageNumber,
        [FromQuery] int pageSize = Configuration.DefaultPageSize
    )
    {
        var request = new GetTransactionsByPeriodRequest
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            UserId =
                user.Identity?.Name
                ?? throw new InvalidOperationException("User ID not found in claims."),
            StartDate = startDate,
            EndDate = endDate,
        };

        var result = await handler.GetByPeriodAsync(request);
        return result.ToResult();
    }
}
