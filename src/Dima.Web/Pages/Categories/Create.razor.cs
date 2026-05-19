using Dima.Core.Handlers;
using Dima.Core.Requests.Categories;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Categories;

public class CreateCategoryPage : ComponentBase
{
    public bool IsBusy { get; set; }
    public bool _isValid;
    public CreateCategoryRequest InputModel { get; set; } = new();

    public MudForm? _mudForm;

    [Inject]
    public ICategoryHandler Handler { get; set; } = null!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    public async Task HandleValidSubmitAsync()
    {
        if (!_isValid)
            return;

        IsBusy = true;

        try
        {
            var result = await Handler.CreateAsync(InputModel);

            if (result.IsSuccess)
            {
                Snackbar.Add("Category created successfully.", Severity.Success);
                NavigationManager.NavigateTo("/categories");
            }
        }
        finally
        {
            IsBusy = false;
        }
    }
}
