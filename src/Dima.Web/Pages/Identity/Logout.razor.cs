using Dima.Core.Handlers;
using Dima.Web.Security;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Identity;

public partial class Logout : ComponentBase
{
    [Inject]
    ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    IAccountHandler Handler { get; set; } = null!;

    [Inject]
    ICookieAuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        if (await AuthenticationStateProvider.CheckAuthenticatedAsync())
        {
            await Handler.LogoutAsync();
            Snackbar.Add("Logged out successfully.", Severity.Success);
        }

        await base.OnInitializedAsync();
    }
}
