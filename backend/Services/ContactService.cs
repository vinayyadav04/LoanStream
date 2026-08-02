using LoanStream.Api.Data;
using LoanStream.Api.Models;

namespace LoanStream.Api.Services;

public sealed class ContactService : IContactService
{
    private readonly IContactRepository _repository;

    public ContactService(IContactRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> SubmitContactAsync(ContactSubmissionRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Phone) ||
            string.IsNullOrWhiteSpace(request.Message))
        {
            return false;
        }

        await _repository.InsertAsync(new ContactRecord
        {
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            Topic = request.Topic,
            Message = request.Message,
            CreatedDate = DateTime.UtcNow
        });

        return true;
    }
}
