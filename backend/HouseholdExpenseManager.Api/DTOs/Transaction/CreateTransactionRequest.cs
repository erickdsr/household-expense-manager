using System.ComponentModel.DataAnnotations;
using HouseholdExpenseManager.Api.Models.Enums;

namespace HouseholdExpenseManager.Api.DTOs.Transaction;

public class CreateTransactionRequest
{
    [Required]
    public string Description { get; set; } = string.Empty;

    [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
    public decimal Amount { get; set; }

    public TransactionType Type { get; set; }

    [Range(1, int.MaxValue)]
    public int PersonId { get; set; }
}
