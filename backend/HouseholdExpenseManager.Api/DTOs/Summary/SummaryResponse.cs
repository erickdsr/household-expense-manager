namespace HouseholdExpenseManager.Api.DTOs.Summary;

public class SummaryResponse
{
    public List<PersonSummaryResponse> People { get; set; } = new List<PersonSummaryResponse>();

    public GeneralSummaryResponse General { get; set; } = new GeneralSummaryResponse();
}
