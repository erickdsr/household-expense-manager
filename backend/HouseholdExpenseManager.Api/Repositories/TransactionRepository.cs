using HouseholdExpenseManager.Api.Data.Context;
using HouseholdExpenseManager.Api.Models.Entities;
using HouseholdExpenseManager.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HouseholdExpenseManager.Api.Repositories;

// Centraliza consultas de transacoes e carrega Person quando a UI precisa do nome da pessoa.
public class TransactionRepository(AppDbContext context) : ITransactionRepository
{
    public async Task<List<FinancialTransaction>> GetAllAsync()
    {
        return await context.Transactions
            .Include(transaction => transaction.Person)
            .OrderByDescending(transaction => transaction.CreatedAt)
            .ToListAsync();
    }

    public async Task<FinancialTransaction?> GetByIdAsync(int id)
    {
        return await context.Transactions
            .Include(transaction => transaction.Person)
            .FirstOrDefaultAsync(transaction => transaction.Id == id);
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
