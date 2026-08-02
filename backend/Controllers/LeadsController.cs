using LoanStream.Api.Models;
using LoanStream.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoanStream.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class LeadsController : ControllerBase
{
    private readonly ILeadIngestionService _leadIngestionService;

    public LeadsController(ILeadIngestionService leadIngestionService)
    {
        _leadIngestionService = leadIngestionService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] LeadSubmissionRequest request)
    {
        var success = await _leadIngestionService.SubmitLeadAsync(request);
        if (!success)
        {
            return BadRequest(new { message = "Please provide the required details." });
        }

        return Ok(new { success = true, message = "Lead accepted and saved successfully." });
    }
}
