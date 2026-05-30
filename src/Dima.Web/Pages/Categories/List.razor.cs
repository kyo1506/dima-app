using Dima.Core.Handlers;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses.Categories;
using Dima.Web.Components;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Categories;

public partial class ListCategoriesPage : ComponentBase
{
    public bool IsBusy { get; set; } = false;
    public List<CategoryResponse> _categories = [];
    public string SearchTerm { get; set; } = string.Empty;

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    public ICategoryHandler Handler { get; set; } = null!;

    [Inject]
    public IDialogService DialogService { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        IsBusy = true;

        try
        {
            var request = new GetAllCategoriesRequest();

            var response = await Handler.GetAllAsync(request);

            if (!response.IsSuccess)
            {
                Snackbar.Add("Failed to load categories", Severity.Error);
                return;
            }

            _categories = response.Data ?? [];
        }
        finally
        {
            IsBusy = false;
        }
    }

    public async Task OnDeleteButtonClickedAsync(long id, string title)
    {
        var parameters = new DialogParameters<ConfirmDialog>
        {
            { x => x.ContentText, $"Are you sure you want to delete the category '{title}'?" },
            { x => x.ButtonText, "Delete" },
            { x => x.Color, Color.Error },
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small };

        var dialog = await DialogService.ShowAsync<ConfirmDialog>(
            "Delete Category",
            parameters,
            options
        );
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            var deleteResponse = await Handler.DeleteAsync(new DeleteCategoryRequest { Id = id });

            if (deleteResponse.IsSuccess)
            {
                Snackbar.Add("Category deleted successfully.", Severity.Success);
                _categories.RemoveAll(c => c.Id == id);
                StateHasChanged();
            }
        }
    }

    public Func<CategoryResponse, bool> FilterCategories =>
        category =>
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
                return true;

            return category.Title.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)
                || category.Id.ToString().Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)
                || (
                    category.Description?.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)
                    ?? false
                );
        };
}
