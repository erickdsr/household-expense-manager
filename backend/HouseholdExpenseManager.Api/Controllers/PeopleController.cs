using HouseholdExpenseManager.Api.Common;
using HouseholdExpenseManager.Api.DTOs.Person;
using HouseholdExpenseManager.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HouseholdExpenseManager.Api.Controllers;

[ApiController]
[Route("api/people")]
public class PeopleController(IPersonService personService) : ControllerBase
{
    /// <summary>
    /// Retorna todas as pessoas cadastradas ordenadas pelo identificador.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<PersonResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<PersonResponse>>> GetAllAsync()
    {
        var people = await personService.GetAllAsync();

        return Ok(people);
    }

    /// <summary>
    /// Retorna uma pessoa pelo id.
    /// </summary>
    [HttpGet("{id:int}", Name = nameof(GetPersonByIdAsync))]
    [ProducesResponseType(typeof(PersonResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PersonResponse>> GetPersonByIdAsync(int id)
    {
        var person = await personService.GetByIdAsync(id);

        return Ok(person);
    }

    /// <summary>
    /// Cria uma pessoa com nome e idade.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(PersonResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PersonResponse>> CreateAsync(CreatePersonRequest request)
    {
        var person = await personService.CreateAsync(request);

        return CreatedAtRoute(nameof(GetPersonByIdAsync), new { id = person.Id }, person);
    }

    /// <summary>
    /// Exclui uma pessoa pelo id e remove as transacoes relacionadas por cascade delete.
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await personService.DeleteAsync(id);

        return NoContent();
    }
}
