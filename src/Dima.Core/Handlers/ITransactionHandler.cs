using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using Dima.Core.Responses.Transactions;

namespace Dima.Core.Handlers;

public interface ITransactionHandler
{
    Task<BaseResponse<TransactionResponse?>> GetByIdAsync(GetTransactionByIdRequest request);
    Task<PagedResponse<List<TransactionResponse>>> GetByPeriodAsync(
        GetTransactionsByPeriodRequest request
    );
    Task<BaseResponse<TransactionResponse?>> CreateAsync(CreateTransactionRequest request);
    Task<BaseResponse<TransactionResponse?>> UpdateAsync(UpdateTransactionRequest request);
    Task<BaseResponse<TransactionResponse?>> DeleteAsync(DeleteTransactionRequest request);
}
