namespace LoanStream.Api.Models;

public sealed class LeadRecord
{
    public Guid Id { get; set; }          // UUID

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string EmploymentType { get; set; } = string.Empty;

    public string MonthlyIncome { get; set; } =string.Empty;

    public float LoanAmount { get; set; }       // float4

    public string City { get; set; } = string.Empty;

    public string Source { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; }

    public string Status { get; set; } = "Pending";
}