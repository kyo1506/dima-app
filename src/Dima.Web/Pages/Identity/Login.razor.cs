using Dima.Core.Handlers;
using Dima.Core.Requests.Account;
using Dima.Web.Security;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Identity;

public partial class Login : ComponentBase
{
    public MudForm? _mudForm;

    [Inject]
    ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    IAccountHandler Handler { get; set; } = null!;

    [Inject]
    NavigationManager Navigation { get; set; } = null!;

    [Inject]
    ICookieAuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

    public bool IsBusy { get; set; } = false;
    public LoginRequest InputModel { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        if (authState.User.Identity is { IsAuthenticated: true })
            Navigation.NavigateTo("/");
    }

    public async Task OnValidSubmitAsync()
    {
        if (_mudForm is null)
            return;

        await _mudForm.ValidateAsync();

        if (!_mudForm.IsValid)
            return;

        IsBusy = true;

        try
        {
            var result = await Handler.LoginAsync(InputModel);

            if (result.IsSuccess)
            {
                Navigation.NavigateTo("/", forceLoad: true);
                return;
            }

            Snackbar.Add(result.Message ?? "An error occurred.", Severity.Error);
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
            throw;
        }
        finally
        {
            IsBusy = false;
        }
    }
}
