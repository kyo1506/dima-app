using System.ComponentModel.DataAnnotations;

namespace Dima.Core.Requests.Transactions;

public class CreateTransactionRequest : BaseRequest
{
    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    [Range(1, 2, ErrorMessage = "Type must be 1 (Deposit) or 2 (Withdrawal)")]
    public int Type { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [Required]
    public long CategoryId { get; set; }

    public DateTime? PaidOrReceivedAt { get; set; }
}
