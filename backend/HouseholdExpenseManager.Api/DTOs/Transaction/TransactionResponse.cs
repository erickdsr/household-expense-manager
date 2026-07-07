using HouseholdExpenseManager.Api.Models.Enums;

namespace HouseholdExpenseManager.Api.DTOs.Transaction;

public class TransactionResponse
{
    public int Id { get; set; }

    public string Description { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public TransactionType Type { get; set; }

    public int PersonId { get; set; }

    public string PersonName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}
