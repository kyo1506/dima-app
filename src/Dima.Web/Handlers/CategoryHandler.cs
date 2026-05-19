using System.Net.Http.Json;
using Dima.Core.Handlers;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Dima.Core.Responses.Categories;

namespace Dima.Web.Handlers;

public class CategoryHandler(IHttpClientFactory httpClientFactory) : ICategoryHandler
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient(
        Configuration.HttpClientName
    );

    public async Task<BaseResponse<CategoryResponse?>> CreateAsync(CreateCategoryRequest request)
    {
        var result = await _httpClient.PostAsJsonAsync("/v1/categories", request);
        return await result.Content.ReadFromJsonAsync<BaseResponse<CategoryResponse?>>()
            ?? new BaseResponse<CategoryResponse?>(null, 400, "Failed to parse response");
    }

    public async Task<BaseResponse<CategoryResponse?>> DeleteAsync(DeleteCategoryRequest request)
    {
        var result = await _httpClient.DeleteAsync($"/v1/categories/{request.Id}");
        return await result.Content.ReadFromJsonAsync<BaseResponse<CategoryResponse?>>()
            ?? new BaseResponse<CategoryResponse?>(null, 400, "Failed to parse response");
    }

    public async Task<PagedResponse<List<CategoryResponse>>> GetAllAsync(
        GetAllCategoriesRequest request
    )
    {
        var result = await _httpClient.GetAsync(
            $"/v1/categories?PageNumber={request.PageNumber}&PageSize={request.PageSize}"
        );
        return await result.Content.ReadFromJsonAsync<PagedResponse<List<CategoryResponse>>>()
            ?? new PagedResponse<List<CategoryResponse>>(null, 400, "Failed to parse response");
    }

    public async Task<BaseResponse<CategoryResponse?>> GetByIdAsync(GetCategoryByIdRequest request)
    {
        var result = await _httpClient.GetAsync($"/v1/categories/{request.Id}");
        return await result.Content.ReadFromJsonAsync<BaseResponse<CategoryResponse?>>()
            ?? new BaseResponse<CategoryResponse?>(null, 400, "Failed to parse response");
    }

    public async Task<BaseResponse<CategoryResponse?>> UpdateAsync(UpdateCategoryRequest request)
    {
        var result = await _httpClient.PutAsJsonAsync($"/v1/categories/{request.Id}", request);
        return await result.Content.ReadFromJsonAsync<BaseResponse<CategoryResponse?>>()
            ?? new BaseResponse<CategoryResponse?>(null, 400, "Failed to parse response");
    }
}
