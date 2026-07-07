using HouseholdExpenseManager.Api.Data.Context;
using HouseholdExpenseManager.Api.Models.Entities;
using HouseholdExpenseManager.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HouseholdExpenseManager.Api.Repositories;

public class TransactionRepository(AppDbContext context) : ITransactionRepository
{
    public async Task<List<FinancialTransaction>> GetAllAsync()
    {
        return await context.Transactions
            .Include(transaction => transaction.Person)
            .OrderByDescending(transaction => transaction.CreatedAt)
            .ToListAsync();
    }

    public async Task<FinancialTransaction> CreateAsync(FinancialTransaction transaction)
    {
        context.Transactions.Add(transaction);
        await context.SaveChangesAsync();

        return transaction;
    }

    public async Task<List<FinancialTransaction>> GetByPersonIdAsync(int personId)
    {
        return await context.Transactions
            .Include(transaction => transaction.Person)
            .Where(transaction => transaction.PersonId == personId)
            .OrderByDescending(transaction => transaction.CreatedAt)
            .ToListAsync();
    }
}
