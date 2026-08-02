using System.ComponentModel.DataAnnotations;

namespace LoanStream.Api.Models;

public sealed class LeadSubmissionRequest
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required")]
    [RegularExpression(@"^[6-9]\d{9}$",
        ErrorMessage = "Please enter a valid 10-digit Indian mobile number")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Employment Type is required")]
    public string EmploymentType { get; set; } = string.Empty;
    public string MonthlyIncome { get; set; } = string.Empty;
    public float LoanAmount { get; set; } = 0;

    [Required(ErrorMessage = "City is required")]
    public string City { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
}
