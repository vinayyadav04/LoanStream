namespace LoanStream.Api.Models;

public sealed class LeadSubmissionRequest
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string EmploymentType { get; set; } = string.Empty;
    public string MonthlyIncome { get; set; } = string.Empty;
    public decimal LoanAmount { get; set; }
    public string City { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
}
