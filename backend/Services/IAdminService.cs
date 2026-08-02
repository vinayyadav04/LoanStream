using LoanStream.Api.Models;

namespace LoanStream.Api.Services;

public interface IAdminService
{
    Task<IReadOnlyList<LeadRecord>> GetLeadsAsync(string? name = null, string? phone = null, DateTime? fromDate = null, DateTime? toDate = null);
    Task<byte[]> ExportExcelAsync();
    Task<byte[]> ExportCsvAsync();
}
