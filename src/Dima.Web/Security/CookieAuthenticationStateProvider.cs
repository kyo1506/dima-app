using System.Net.Http.Json;
using System.Security.Claims;
using Dima.Core.Models.Account;
using Microsoft.AspNetCore.Components.Authorization;

namespace Dima.Web.Security;

public class CookieAuthenticationStateProvider(IHttpClientFactory httpClientFactory)
    : AuthenticationStateProvider,
        ICookieAuthenticationStateProvider
{
    private readonly HttpClient _client = httpClientFactory.CreateClient(
        Configuration.HttpClientName
    );

    public async Task<bool> CheckAuthenticatedAsync()
    {
        var state = await GetAuthenticationStateAsync();
        return state.User.Identity?.IsAuthenticated ?? false;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var user = await GetUserAsync();

        if (user is null)
            return new AuthenticationState(new ClaimsPrincipal());

        var claims = await GetClaims(user);
        var identity = new ClaimsIdentity(claims, nameof(CookieAuthenticationStateProvider));

        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public void NotifyAuthenticationStateChanged() =>
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

    private async Task<User?> GetUserAsync()
    {
        try
        {
            var response = await _client.GetAsync("v1/identity/manage/info");

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<User>();
        }
        catch { }

        return null;
    }

    private async Task<List<Claim>> GetClaims(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Email),
            new(ClaimTypes.Email, user.Email),
        };

        claims.AddRange(
            user.Claims.Where(x => x.Key != ClaimTypes.Name && x.Key != ClaimTypes.Email)
                .Select(x => new Claim(x.Key, x.Value))
        );

        RoleClaim[]? roles;

        try
        {
            roles = await _client.GetFromJsonAsync<RoleClaim[]>("v1/identity/roles");

            claims.AddRange(
                from role in roles ?? []
                where !string.IsNullOrEmpty(role.Type) && !string.IsNullOrEmpty(role.Value)
                select new Claim(
                    role.Type!,
                    role.Value!,
                    role.ValueType,
                    role.Issuer,
                    role.OriginalIssuer
                )
            );
        }
        catch { }

        return claims;
    }
}
