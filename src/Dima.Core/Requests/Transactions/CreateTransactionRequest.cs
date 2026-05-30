using System.ComponentModel.DataAnnotations;
using Dima.Core.Enums;

namespace Dima.Core.Requests.Transactions;

public class CreateTransactionRequest : BaseRequest
{
    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    [EnumDataType(typeof(ETransactionType))]
    public ETransactionType Type { get; set; } = ETransactionType.Withdrawal;

    [Required]
    public decimal Amount { get; set; }

    [Required]
    public long CategoryId { get; set; }

    public DateTime? PaidOrReceivedAt { get; set; }
}
