using LoanStream.Api.Data;
using LoanStream.Api.Models;

namespace LoanStream.Api.Services;

public sealed class LeadIngestionService : ILeadIngestionService
{
    private readonly ILeadRepository _leadRepository;

    public LeadIngestionService(ILeadRepository leadRepository)
    {
        _leadRepository = leadRepository;
    }

    public async Task<bool> SubmitLeadAsync(LeadSubmissionRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) ||
            string.IsNullOrWhiteSpace(request.Phone) ||
            string.IsNullOrWhiteSpace(request.EmploymentType) ||
            string.IsNullOrWhiteSpace(request.MonthlyIncome) ||
            string.IsNullOrWhiteSpace(request.City))
        {
            return false;
        }

        var lead = new LeadRecord
        {
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            EmploymentType = request.EmploymentType,
            MonthlyIncome = request.MonthlyIncome,
            LoanAmount = request.LoanAmount,
            City = request.City,
            Source = request.Source,
            CreatedDate = DateTime.UtcNow,
            Status = "Pending"
        };

        var insertedId = await _leadRepository.InsertAsync(lead);
        return insertedId > 0;
    }
}
