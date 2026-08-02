using LoanStream.Api.Models;

namespace LoanStream.Api.Data;

public interface ILeadRepository
{
    Task<Guid> InsertAsync(LeadRecord lead);
    Task<IReadOnlyList<LeadRecord>> GetAllAsync(string? name = null, string? phone = null, DateTime? fromDate = null, DateTime? toDate = null);
}
