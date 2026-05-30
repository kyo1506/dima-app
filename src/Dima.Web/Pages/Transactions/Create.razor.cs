using Dima.Core.Handlers;
using Dima.Core.Requests.Categories;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses.Categories;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Transactions;

public partial class CreateTransactionPage : ComponentBase
{
    public bool IsBusy { get; set; }
    public bool _isValid;
    public CreateTransactionRequest InputModel { get; set; } = new();
    public List<CategoryResponse> _categories = [];

    public MudForm? _mudForm;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    public ITransactionHandler Handler { get; set; } = null!;

    [Inject]
    public ICategoryHandler CategoryHandler { get; set; } = null!;

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        var response = await CategoryHandler.GetAllAsync(new GetAllCategoriesRequest());
        if (response.IsSuccess)
            _categories = response.Data ?? [];
    }

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
                Snackbar.Add("Transaction created successfully.", Severity.Success);
                NavigationManager.NavigateTo("/transactions");
            }
        }
        finally
        {
            IsBusy = false;
        }
    }
}
