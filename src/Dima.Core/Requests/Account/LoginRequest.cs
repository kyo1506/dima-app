using System.ComponentModel.DataAnnotations;

namespace Dima.Core.Requests.Account;

public class LoginRequest : BaseRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
