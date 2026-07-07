using System.ComponentModel.DataAnnotations;

namespace HouseholdExpenseManager.Api.DTOs.Person;

public class CreatePersonRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int Age { get; set; }
}
