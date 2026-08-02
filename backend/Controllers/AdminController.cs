using LoanStream.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoanStream.Api.Controllers;

[ApiController]
[Route("api/admin")]
public sealed class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet("leads")]
    public async Task<IActionResult> GetLeads([FromQuery] string? name, [FromQuery] string? phone, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
    {
        var leads = await _adminService.GetLeadsAsync(name, phone, fromDate, toDate);
        return Ok(leads);
    }

    [HttpGet("export/excel")]
    public async Task<IActionResult> ExportExcel()
    {
        var content = await _adminService.ExportExcelAsync();
        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "leads.xlsx");
    }

    [HttpGet("export/csv")]
    public async Task<IActionResult> ExportCsv()
    {
        var content = await _adminService.ExportCsvAsync();
        return File(content, "text/csv", "leads.csv");
    }
}
