using Dima.Core.Models;
using Dima.Core.Responses.Categories;

namespace Dima.Core.Mappers;

public static class CategoryMapper
{
    public static CategoryResponse ToResponse(this Category category) =>
        new()
        {
            Id = category.Id,
            Title = category.Title,
            Description = category.Description,
            UserId = category.UserId,
        };
}
