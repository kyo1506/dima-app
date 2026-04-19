using Dima.Api.Data;
using Dima.Core.Common.Extensions;
using Dima.Core.Enums;
using Dima.Core.Handlers;
using Dima.Core.Mappers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using Dima.Core.Responses.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers;

public class TransactionHandler(AppDbContext context) : ITransactionHandler
{
    public async Task<BaseResponse<TransactionResponse?>> CreateAsync(
        CreateTransactionRequest request
    )
    {
        try
        {
            var transaction = new Transaction
            {
                Title = request.Title,
                PaidOrReceivedAt = request.PaidOrReceivedAt,
                Type = (ETransactionType)request.Type,
                Amount = request.Amount,
                CategoryId = request.CategoryId,
                UserId = request.UserId,
            };

            await context.Transactions.AddAsync(transaction);
            await context.SaveChangesAsync();

            return new BaseResponse<TransactionResponse?>(
                transaction.ToResponse(),
                201,
                "Transaction created successfully"
            );
        }
        catch
        {
            return new BaseResponse<TransactionResponse?>(
                null,
                code: 500,
                message: "An error occurred while creating the transaction"
            );
        }
    }

    public async Task<BaseResponse<TransactionResponse?>> DeleteAsync(
        DeleteTransactionRequest request
    )
    {
        try
        {
            var transaction = await context.Transactions.FirstOrDefaultAsync(t =>
                t.Id == request.Id && t.UserId == request.UserId
            );

            if (transaction == null)
            {
                return new BaseResponse<TransactionResponse?>(
                    null,
                    code: 404,
                    message: "Transaction not found"
                );
            }

            context.Transactions.Remove(transaction);
            await context.SaveChangesAsync();

            return new BaseResponse<TransactionResponse?>(
                transaction.ToResponse(),
                code: 200,
                message: "Transaction deleted successfully"
            );
        }
        catch
        {
            return new BaseResponse<TransactionResponse?>(
                null,
                code: 500,
                message: "An error occurred while deleting the transaction"
            );
        }
    }

    public async Task<BaseResponse<TransactionResponse?>> GetByIdAsync(
        GetTransactionByIdRequest request
    )
    {
        try
        {
            var transaction = await context
                .Transactions.AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == request.Id && t.UserId == request.UserId);

            return transaction is null
                ? new BaseResponse<TransactionResponse?>(
                    null,
                    code: 404,
                    message: "Transaction not found"
                )
                : new BaseResponse<TransactionResponse?>(
                    transaction.ToResponse(),
                    code: 200,
                    message: "Transaction retrieved successfully"
                );
        }
        catch
        {
            return new BaseResponse<TransactionResponse?>(
                null,
                code: 500,
                message: "An error occurred while retrieving the transaction"
            );
        }
    }

    public async Task<PagedResponse<List<TransactionResponse>>> GetByPeriodAsync(
        GetTransactionsByPeriodRequest request
    )
    {
        try
        {
            request.StartDate ??= DateTime.UtcNow.GetFirstDay();
            request.EndDate ??= DateTime.UtcNow.GetLastDay();

            var query = context
                .Transactions.AsNoTracking()
                .Where(t =>
                    t.CreatedAt >= request.StartDate
                    && t.CreatedAt <= request.EndDate
                    && t.UserId == request.UserId
                )
                .OrderBy(t => t.CreatedAt);

            var totalItems = await query.CountAsync();

            var transactions = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(c => c.ToResponse())
                .ToListAsync();

            return new PagedResponse<List<TransactionResponse>>(
                transactions,
                totalItems,
                request.PageNumber,
                request.PageSize
            );
        }
        catch
        {
            return new PagedResponse<List<TransactionResponse>>(
                null,
                500,
                "An error occurred while retrieving transactions"
            );
        }
    }

    public async Task<BaseResponse<TransactionResponse?>> UpdateAsync(
        UpdateTransactionRequest request
    )
    {
        try
        {
            var transaction = await context.Transactions.FirstOrDefaultAsync(t =>
                t.Id == request.Id && t.UserId == request.UserId
            );

            if (transaction == null)
            {
                return new BaseResponse<TransactionResponse?>(
                    null,
                    code: 404,
                    message: "Transaction not found"
                );
            }

            transaction.Title = request.Title;
            transaction.PaidOrReceivedAt = request.PaidOrReceivedAt;
            transaction.Type = (ETransactionType)request.Type;
            transaction.Amount = request.Amount;
            transaction.CategoryId = request.CategoryId;

            context.Transactions.Update(transaction);
            await context.SaveChangesAsync();

            return new BaseResponse<TransactionResponse?>(
                transaction.ToResponse(),
                code: 200,
                message: "Transaction updated successfully"
            );
        }
        catch
        {
            return new BaseResponse<TransactionResponse?>(
                null,
                code: 500,
                message: "An error occurred while updating the transaction"
            );
        }
    }
}
