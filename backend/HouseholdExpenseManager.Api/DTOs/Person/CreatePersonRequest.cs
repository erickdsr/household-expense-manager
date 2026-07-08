using System.ComponentModel.DataAnnotations;

namespace HouseholdExpenseManager.Api.DTOs.Person;

public class CreatePersonRequest
{
    [Required(ErrorMessage = "Name is required.")]
    [MinLength(1, ErrorMessage = "Name is required.")]
    public string Name { get; set; } = string.Empty;

    [Range(0, int.MaxValue, ErrorMessage = "Age cannot be negative.")]
    public int Age { get; set; }
}
