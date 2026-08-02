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
    try
    {
        var success = await _leadIngestionService.SubmitLeadAsync(request);

        return Ok(new
        {
            success,
            request
        });
    }
    catch (Exception ex)
    {
        return StatusCode(500, new
        {
            ex.Message,
            ex.StackTrace
        });
    }
}}
