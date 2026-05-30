using Dima.Core.Handlers;
using Dima.Core.Requests.Categories;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Categories;

public partial class EditCategoryPage : ComponentBase
{
    [Parameter]
    public long Id { get; set; }

    public bool IsBusy { get; set; }
    public bool _isValid;
    public UpdateCategoryRequest InputModel { get; set; } = new();

    public MudForm? _mudForm;

    [Inject]
    public ICategoryHandler Handler { get; set; } = null!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        IsBusy = true;

        try
        {
            var request = new GetCategoryByIdRequest { Id = Id };
            var result = await Handler.GetByIdAsync(request);

            if (!result.IsSuccess || result.Data is null)
            {
                Snackbar.Add(result.Message ?? "Failed to load category details.", Severity.Error);
                NavigationManager.NavigateTo("/categories");
                return;
            }

            InputModel = new UpdateCategoryRequest
            {
                Id = result.Data.Id,
                Title = result.Data.Title,
                Description = result.Data.Description,
            };
        }
        finally
        {
            IsBusy = false;
        }
    }

    public async Task HandleValidSubmitAsync()
    {
        if (!_isValid)
            return;

        IsBusy = true;

        try
        {
            var result = await Handler.UpdateAsync(InputModel);

            if (result.IsSuccess)
            {
                Snackbar.Add("Category updated successfully.", Severity.Success);
                NavigationManager.NavigateTo("/categories");
            }
        }
        finally
        {
            IsBusy = false;
        }
    }
}
