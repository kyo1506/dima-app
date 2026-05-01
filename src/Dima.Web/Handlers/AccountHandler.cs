using System.Net.Http.Json;
using Dima.Core.Handlers;
using Dima.Core.Requests.Account;
using Dima.Core.Responses;

namespace Dima.Web.Handlers;

public class AccountHandler(IHttpClientFactory httpClientFactory) : IAccountHandler
{
    private readonly HttpClient _client = httpClientFactory.CreateClient(
        Configuration.HttpClientName
    );

    public async Task<BaseResponse<string>> LoginAsync(LoginRequest request)
    {
        try
        {
            var result = await _client.PostAsJsonAsync(
                "v1/identity/login?useCookies=true",
                request
            );

            return result.IsSuccessStatusCode
                ? new BaseResponse<string>(null, 200, "Login successful")
                : new BaseResponse<string>(
                    null,
                    (int)result.StatusCode,
                    result.ReasonPhrase ?? "Could not login"
                );
        }
        catch
        {
            return new BaseResponse<string>(null, 500, "An unexpected error occurred");
        }
    }

    public async Task LogoutAsync()
    {
        await _client.PostAsync("v1/identity/logout", null);
    }

    public async Task<BaseResponse<string>> RegisterAsync(RegisterRequest request)
    {
        try
        {
            var result = await _client.PostAsJsonAsync("v1/identity/register", request);

            return result.IsSuccessStatusCode
                ? new BaseResponse<string>(null, 201, "User registered successfully")
                : new BaseResponse<string>(
                    null,
                    (int)result.StatusCode,
                    result.ReasonPhrase ?? "Could not register"
                );
        }
        catch
        {
            return new BaseResponse<string>(null, 500, "An unexpected error occurred");
        }
    }
}
