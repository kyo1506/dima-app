using System.Net.Http.Json;
using Dima.Core.Common.Extensions;
using Dima.Core.Handlers;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using Dima.Core.Responses.Transactions;

namespace Dima.Web.Handlers;

public class TransactionHandler(IHttpClientFactory httpClientFactory) : ITransactionHandler
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient(
        Configuration.HttpClientName
    );

    public async Task<BaseResponse<TransactionResponse?>> CreateAsync(
        CreateTransactionRequest request
    )
    {
        var result = await _httpClient.PostAsJsonAsync("/v1/transactions", request);
        return await result.Content.ReadFromJsonAsync<BaseResponse<TransactionResponse?>>()
            ?? new BaseResponse<TransactionResponse?>(null, 400, "Failed to parse response");
    }

    public async Task<BaseResponse<TransactionResponse?>> DeleteAsync(
        DeleteTransactionRequest request
    )
    {
        var result = await _httpClient.DeleteAsync($"/v1/transactions/{request.Id}");
        return await result.Content.ReadFromJsonAsync<BaseResponse<TransactionResponse?>>()
            ?? new BaseResponse<TransactionResponse?>(null, 400, "Failed to parse response");
    }

    public async Task<BaseResponse<TransactionResponse?>> GetByIdAsync(
        GetTransactionByIdRequest request
    )
    {
        var result = await _httpClient.GetAsync($"/v1/transactions/{request.Id}");
        return await result.Content.ReadFromJsonAsync<BaseResponse<TransactionResponse?>>()
            ?? new BaseResponse<TransactionResponse?>(null, 400, "Failed to parse response");
    }

    public async Task<PagedResponse<List<TransactionResponse>>> GetByPeriodAsync(
        GetTransactionsByPeriodRequest request
    )
    {
        const string format = "yyyy-MM-dd";

        var startDate = request.StartDate is not null
            ? request.StartDate.Value.ToString(format)
            : DateTime.Now.GetFirstDay().ToString(format);

        var endDate = request.EndDate is not null
            ? request.EndDate.Value.ToString(format)
            : DateTime.Now.GetLastDay().ToString(format);

        var result = await _httpClient.GetAsync(
            $"/v1/transactions?StartDate={startDate}&EndDate={endDate}&PageNumber={request.PageNumber}&PageSize={request.PageSize}"
        );

        return await result.Content.ReadFromJsonAsync<PagedResponse<List<TransactionResponse>>>()
            ?? new PagedResponse<List<TransactionResponse>>(null, 400, "Failed to parse response");
    }

    public async Task<BaseResponse<TransactionResponse?>> UpdateAsync(
        UpdateTransactionRequest request
    )
    {
        var result = await _httpClient.PutAsJsonAsync($"/v1/transactions/{request.Id}", request);
        return await result.Content.ReadFromJsonAsync<BaseResponse<TransactionResponse?>>()
            ?? new BaseResponse<TransactionResponse?>(null, 400, "Failed to parse response");
    }
}
