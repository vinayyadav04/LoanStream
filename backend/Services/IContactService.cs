using LoanStream.Api.Models;

namespace LoanStream.Api.Services;

public interface IContactService
{
    Task<bool> SubmitContactAsync(ContactSubmissionRequest request);
}
