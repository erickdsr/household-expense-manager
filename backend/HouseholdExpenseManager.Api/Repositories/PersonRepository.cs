using HouseholdExpenseManager.Api.Data.Context;
using HouseholdExpenseManager.Api.Models.Entities;
using HouseholdExpenseManager.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HouseholdExpenseManager.Api.Repositories;

// Mantem as consultas EF Core de pessoas isoladas das regras de negocio da camada de servico.
public class PersonRepository(AppDbContext context) : IPersonRepository
{
    public async Task<List<Person>> GetAllAsync()
    {
        return await context.People
            .OrderBy(person => person.Id)
            .ToListAsync();
    }

    public async Task<Person?> GetByIdAsync(int id)
    {
        return await context.People
            .FirstOrDefaultAsync(person => person.Id == id);
    }

    public async Task<Person> CreateAsync(Person person)
    {
        context.People.Add(person);
        await context.SaveChangesAsync();

        return person;
    }

    public async Task DeleteAsync(Person person)
    {
        // O cascade delete configurado no AppDbContext tambem remove as transacoes desta pessoa.
        context.People.Remove(person);
        await context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await context.People
            .AnyAsync(person => person.Id == id);
    }
}
