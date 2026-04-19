using Dima.Core.Models;
using Dima.Core.Responses.Transactions;

namespace Dima.Core.Mappers;

public static class TransactionMapper
{
    public static TransactionResponse ToResponse(this Transaction transaction) =>
        new()
        {
            Id = transaction.Id,
            Title = transaction.Title,
            CreatedAt = transaction.CreatedAt,
            PaidOrReceivedAt = transaction.PaidOrReceivedAt,
            Type = (int)transaction.Type,
            Amount = transaction.Amount,
            CategoryId = transaction.CategoryId,
            UserId = transaction.UserId,
        };
}
