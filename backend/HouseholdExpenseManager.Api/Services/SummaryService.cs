using HouseholdExpenseManager.Api.DTOs.Summary;
using HouseholdExpenseManager.Api.Models.Enums;
using HouseholdExpenseManager.Api.Repositories.Interfaces;
using HouseholdExpenseManager.Api.Services.Interfaces;

namespace HouseholdExpenseManager.Api.Services;

// Monta os totais financeiros somente leitura usados pelo dashboard.
public class SummaryService(
    IPersonRepository personRepository,
    ITransactionRepository transactionRepository) : ISummaryService
{
    public async Task<SummaryResponse> GetSummaryAsync()
    {
        var people = await personRepository.GetAllAsync();
        var personSummaries = new List<PersonSummaryResponse>();

        decimal generalIncome = 0;
        decimal generalExpenses = 0;

        foreach (var person in people)
        {
            var transactions = await transactionRepository.GetByPersonIdAsync(person.Id);

            // Renda e despesas sao calculadas separadamente para cada pessoa.
            var totalIncome = transactions
                .Where(transaction => transaction.Type == TransactionType.Income)
                .Sum(transaction => transaction.Amount);

            var totalExpenses = transactions
                .Where(transaction => transaction.Type == TransactionType.Expense)
                .Sum(transaction => transaction.Amount);

            // Saldo e o resultado da subtracao das despesas da renda.
            var balance = totalIncome - totalExpenses;

            generalIncome += totalIncome;
            generalExpenses += totalExpenses;

            personSummaries.Add(new PersonSummaryResponse
            {
                PersonId = person.Id,
                PersonName = person.Name,
                TotalIncome = totalIncome,
                TotalExpenses = totalExpenses,
                Balance = balance
            });
        }

        return new SummaryResponse
        {
            People = personSummaries,
            General = new GeneralSummaryResponse
            {
                TotalIncome = generalIncome,
                TotalExpenses = generalExpenses,
                NetBalance = generalIncome - generalExpenses
            }
        };
    }
}
