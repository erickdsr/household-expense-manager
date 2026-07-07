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
    /// Returns all registered people ordered by identifier.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<PersonResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<PersonResponse>>> GetAllAsync()
    {
        var people = await personService.GetAllAsync();

        return Ok(people);
    }

    /// <summary>
    /// Creates a person with name and age.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(PersonResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PersonResponse>> CreateAsync(CreatePersonRequest request)
    {
        var person = await personService.CreateAsync(request);

        return CreatedAtAction(nameof(GetAllAsync), new { id = person.Id }, person);
    }

    /// <summary>
    /// Deletes a person by id and removes related transactions by cascade delete.
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
