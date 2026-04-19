using Dima.Api.Data;
using Dima.Core.Handlers;
using Dima.Core.Mappers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Dima.Core.Responses.Categories;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers;

public class CategoryHandler(AppDbContext context) : ICategoryHandler
{
    public async Task<BaseResponse<CategoryResponse?>> CreateAsync(CreateCategoryRequest request)
    {
        try
        {
            var category = new Category
            {
                UserId = request.UserId,
                Title = request.Title,
                Description = request.Description,
            };

            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            return new BaseResponse<CategoryResponse?>(
                category.ToResponse(),
                201,
                "Category created successfully"
            );
        }
        catch
        {
            return new BaseResponse<CategoryResponse?>(
                null,
                code: 500,
                message: "An error occurred while creating the category"
            );
        }
    }

    public async Task<BaseResponse<CategoryResponse?>> DeleteAsync(DeleteCategoryRequest request)
    {
        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(c =>
                c.Id == request.Id && c.UserId == request.UserId
            );

            if (category == null)
                return new BaseResponse<CategoryResponse?>(
                    null,
                    code: 404,
                    message: "Category not found"
                );

            context.Categories.Remove(category);
            await context.SaveChangesAsync();

            return new BaseResponse<CategoryResponse?>(
                category.ToResponse(),
                message: "Category deleted successfully"
            );
        }
        catch
        {
            return new BaseResponse<CategoryResponse?>(
                null,
                code: 500,
                message: "An error occurred while deleting the category"
            );
        }
    }

    public async Task<PagedResponse<List<CategoryResponse>>> GetAllAsync(
        GetAllCategoriesRequest request
    )
    {
        try
        {
            var query = context
                .Categories.AsNoTracking()
                .Where(c => c.UserId == request.UserId)
                .OrderBy(x => x.Title);

            var totalItems = await query.CountAsync();

            var categories = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(c => c.ToResponse())
                .ToListAsync();

            return new PagedResponse<List<CategoryResponse>>(
                categories,
                totalItems,
                request.PageNumber,
                request.PageSize
            );
        }
        catch
        {
            return new PagedResponse<List<CategoryResponse>>(
                null,
                500,
                "An error occurred while retrieving categories"
            );
        }
    }

    public async Task<BaseResponse<CategoryResponse?>> GetByIdAsync(GetCategoryByIdRequest request)
    {
        try
        {
            var category = await context
                .Categories.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == request.Id && c.UserId == request.UserId);

            return category is null
                ? new BaseResponse<CategoryResponse?>(
                    null,
                    code: 404,
                    message: "Category not found"
                )
                : new BaseResponse<CategoryResponse?>(category.ToResponse());
        }
        catch
        {
            return new BaseResponse<CategoryResponse?>(
                null,
                code: 500,
                message: "An error occurred while retrieving the category"
            );
        }
    }

    public async Task<BaseResponse<CategoryResponse?>> UpdateAsync(UpdateCategoryRequest request)
    {
        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(c =>
                c.Id == request.Id && c.UserId == request.UserId
            );

            if (category == null)
                return new BaseResponse<CategoryResponse?>(
                    null,
                    code: 404,
                    message: "Category not found"
                );

            category.Title = request.Title;
            category.Description = request.Description;

            context.Categories.Update(category);
            await context.SaveChangesAsync();

            return new BaseResponse<CategoryResponse?>(
                category.ToResponse(),
                message: "Category updated successfully"
            );
        }
        catch
        {
            return new BaseResponse<CategoryResponse?>(
                null,
                code: 500,
                message: "[FP079] An error occurred while updating the category"
            );
        }
    }
}
