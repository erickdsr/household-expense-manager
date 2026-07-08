using System.ComponentModel.DataAnnotations;
using HouseholdExpenseManager.Api.Models.Enums;

namespace HouseholdExpenseManager.Api.DTOs.Transaction;

public class CreateTransactionRequest
{
    [Required(ErrorMessage = "Description is required.")]
    [MinLength(1, ErrorMessage = "Description is required.")]
    public string Description { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Transaction type is required.")]
    public TransactionType? Type { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Person is required.")]
    public int PersonId { get; set; }
}
