using System.Text;
using LoanStream.Api.Data;
using LoanStream.Api.Models;

namespace LoanStream.Api.Services;

public sealed class AdminService : IAdminService
{
    private readonly ILeadRepository _leadRepository;

    public AdminService(ILeadRepository leadRepository)
    {
        _leadRepository = leadRepository;
    }

    public Task<IReadOnlyList<LeadRecord>> GetLeadsAsync(string? name = null, string? phone = null, DateTime? fromDate = null, DateTime? toDate = null)
        => _leadRepository.GetAllAsync(name, phone, fromDate, toDate);

    public async Task<byte[]> ExportExcelAsync()
    {
        var leads = await _leadRepository.GetAllAsync();
        var builder = new StringBuilder();
        builder.AppendLine("Id,Name,Email,Phone,EmploymentType,MonthlyIncome,LoanAmount,City,Source,CreatedDate,Status");
        foreach (var lead in leads)
        {
            builder.AppendLine($"{lead.Id},{EscapeCsv(lead.Name)},{EscapeCsv(lead.Email)},{EscapeCsv(lead.Phone)},{EscapeCsv(lead.EmploymentType)},{EscapeCsv(lead.MonthlyIncome.ToString())},{lead.LoanAmount},{EscapeCsv(lead.City)},{EscapeCsv(lead.Source)},{lead.CreatedDate:O},{EscapeCsv(lead.Status)}");
        }

        return Encoding.UTF8.GetBytes(builder.ToString());
    }

    public async Task<byte[]> ExportCsvAsync()
    {
        return await ExportExcelAsync();
    }

    private static string EscapeCsv(string value) => $"\"{value.Replace("\"", "\"\"")}\"";
}
