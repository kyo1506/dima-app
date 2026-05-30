using System.ComponentModel.DataAnnotations;

namespace Dima.Core.Requests.Categories;

public class CreateCategoryRequest : BaseRequest
{
    [Required]
    [MaxLength(80)]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }
}
