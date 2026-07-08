using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HouseholdExpenseManager.Api.Models.Enums;

namespace HouseholdExpenseManager.Api.Models.Entities;

// Representa uma renda ou despesa registrada para uma pessoa.
public class FinancialTransaction
{
    public int Id { get; set; }

    [Required]
    public string Description { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public TransactionType Type { get; set; }

    public int PersonId { get; set; }

    [ForeignKey(nameof(PersonId))]
    public Person Person { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}
