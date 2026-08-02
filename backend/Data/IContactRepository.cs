using LoanStream.Api.Models;

namespace LoanStream.Api.Data;

public interface IContactRepository
{
    Task<int> InsertAsync(ContactRecord contact);
}
