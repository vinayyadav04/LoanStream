using LoanStream.Api.Models;
using LoanStream.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoanStream.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ContactController : ControllerBase
{
    private readonly IContactService _contactService;

    public ContactController(IContactService contactService)
    {
        _contactService = contactService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ContactSubmissionRequest request)
    {
        var success = await _contactService.SubmitContactAsync(request);
        if (!success)
        {
            return BadRequest(new { message = "Please fill all required fields." });
        }

        return Ok(new { success = true, message = "Contact request saved." });
    }
}
