using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Dima.Core.Requests;

public abstract class BaseRequest
{
    [Required]
    [JsonIgnore]
    public string UserId { get; set; } = string.Empty;
}
