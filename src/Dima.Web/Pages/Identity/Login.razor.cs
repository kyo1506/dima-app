using Dima.Core.Handlers;
using Dima.Core.Requests.Account;
using Dima.Web.Security;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Identity;

public partial class Login : ComponentBase
{
    public MudForm? _mudForm;
    public bool _isValid;

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
        if (!_isValid)
            return;

        IsBusy = true;

        try
        {
            var result = await Handler.LoginAsync(InputModel);

            if (result.IsSuccess)
                Navigation.NavigateTo("/", forceLoad: true);
        }
        finally
        {
            IsBusy = false;
        }
    }
}