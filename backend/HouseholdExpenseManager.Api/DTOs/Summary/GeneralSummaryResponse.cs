namespace HouseholdExpenseManager.Api.DTOs.Summary;

public class GeneralSummaryResponse
{
    public decimal TotalIncome { get; set; }

    public decimal TotalExpenses { get; set; }

    public decimal NetBalance { get; set; }
}
