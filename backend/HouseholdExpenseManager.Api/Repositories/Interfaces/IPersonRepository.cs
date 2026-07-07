using HouseholdExpenseManager.Api.Models.Entities;

namespace HouseholdExpenseManager.Api.Repositories.Interfaces;

public interface IPersonRepository
{
    Task<List<Person>> GetAllAsync();

    Task<Person?> GetByIdAsync(int id);

    Task<Person> CreateAsync(Person person);

    Task DeleteAsync(Person person);

    Task<bool> ExistsAsync(int id);
}
