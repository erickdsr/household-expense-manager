using System.ComponentModel.DataAnnotations;

namespace HouseholdExpenseManager.Api.Models.Entities;

// Representa uma pessoa que possui transacoes financeiras.
public class Person
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public int Age { get; set; }

    public ICollection<FinancialTransaction> Transactions { get; set; } = new List<FinancialTransaction>();
}
