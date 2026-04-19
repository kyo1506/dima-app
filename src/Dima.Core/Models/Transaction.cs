using Dima.Core.Enums;

namespace Dima.Core.Models;

public class Transaction : BaseEntity
{
    public long CategoryId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? PaidOrReceivedAt { get; set; }
    public ETransactionType Type { get; set; } = ETransactionType.Withdrawal;
    public decimal Amount { get; set; }
    public Category Category { get; set; } = null!;
}
