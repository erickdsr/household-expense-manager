using HouseholdExpenseManager.Api.DTOs.Person;
using HouseholdExpenseManager.Api.Exceptions;
using HouseholdExpenseManager.Api.Models.Entities;
using HouseholdExpenseManager.Api.Repositories.Interfaces;
using HouseholdExpenseManager.Api.Services.Interfaces;

namespace HouseholdExpenseManager.Api.Services;

public class PersonService(IPersonRepository personRepository) : IPersonService
{
    public async Task<List<PersonResponse>> GetAllAsync()
    {
        var people = await personRepository.GetAllAsync();

        return people.Select(MapToResponse).ToList();
    }

    public async Task<PersonResponse> CreateAsync(CreatePersonRequest request)
    {
        // Creates a person from the request data and persists it through the repository.
        var person = new Person
        {
            Name = request.Name,
            Age = request.Age
        };

        var createdPerson = await personRepository.CreateAsync(person);

        return MapToResponse(createdPerson);
    }

    public async Task DeleteAsync(int id)
    {
        var person = await personRepository.GetByIdAsync(id);

        if (person is null)
        {
            throw new NotFoundException("Person not found.");
        }

        // Related transactions are removed by the EF Core cascade delete configuration.
        await personRepository.DeleteAsync(person);
    }

    private static PersonResponse MapToResponse(Person person)
    {
        return new PersonResponse
        {
            Id = person.Id,
            Name = person.Name,
            Age = person.Age
        };
    }
}
