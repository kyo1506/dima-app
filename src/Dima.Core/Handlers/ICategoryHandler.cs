using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Dima.Core.Responses.Categories;

namespace Dima.Core.Handlers;

public interface ICategoryHandler
{
    Task<PagedResponse<List<CategoryResponse>>> GetAllAsync(GetAllCategoriesRequest request);
    Task<BaseResponse<CategoryResponse?>> GetByIdAsync(GetCategoryByIdRequest request);
    Task<BaseResponse<CategoryResponse?>> CreateAsync(CreateCategoryRequest request);
    Task<BaseResponse<CategoryResponse?>> UpdateAsync(UpdateCategoryRequest request);
    Task<BaseResponse<CategoryResponse?>> DeleteAsync(DeleteCategoryRequest request);
}
