using Dima.Core.Requests.Account;
using Dima.Core.Responses;

namespace Dima.Core.Handlers;

public interface IAccountHandler
{
    Task<BaseResponse<string>> LoginAsync(LoginRequest request);
    Task<BaseResponse<string>> RegisterAsync(RegisterRequest request);
    Task LogoutAsync();
}
