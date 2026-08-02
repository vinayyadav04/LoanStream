using LoanStream.Api.Models;

namespace LoanStream.Api.Services;

public interface ILeadIngestionService
{
    Task<bool> SubmitLeadAsync(LeadSubmissionRequest request);
}
