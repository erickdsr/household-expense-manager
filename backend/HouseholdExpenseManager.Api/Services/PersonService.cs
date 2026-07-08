using System.ComponentModel.DataAnnotations;
using HouseholdExpenseManager.Api.DTOs.Person;
using HouseholdExpenseManager.Api.Exceptions;
using HouseholdExpenseManager.Api.Models.Entities;
using HouseholdExpenseManager.Api.Repositories.Interfaces;
using HouseholdExpenseManager.Api.Services.Interfaces;

namespace HouseholdExpenseManager.Api.Services;

// Cuida da validacao e da orquestracao de pessoas antes dos dados chegarem ao repositorio.
public class PersonService(IPersonRepository personRepository) : IPersonService
{
    public async Task<List<PersonResponse>> GetAllAsync()
    {
        var people = await personRepository.GetAllAsync();

        return people.Select(MapToResponse).ToList();
    }

    public async Task<PersonResponse> GetByIdAsync(int id)
    {
        var person = await personRepository.GetByIdAsync(id);

        if (person is null)
        {
            throw new NotFoundException("Person not found.");
        }

        return MapToResponse(person);
    }

    public async Task<PersonResponse> CreateAsync(CreatePersonRequest request)
    {
        // Normaliza a entrada do usuario antes de validar e salvar.
        var name = request.Name.Trim();

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ValidationException("Name is required.");
        }

        if (request.Age < 0)
        {
            throw new ValidationException("Age cannot be negative.");
        }

        var person = new Person
        {
            Name = name,
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

        // As transacoes relacionadas sao removidas pela configuracao de cascade delete do EF Core.
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
