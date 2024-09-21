using Gifty.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gifty.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class BirthdayReminderController : ControllerBase
{
    private readonly BirthdayReminderService _birthdayReminderService;

    public BirthdayReminderController(BirthdayReminderService birthdayReminderService)
    {
        _birthdayReminderService = birthdayReminderService;
    }

    [HttpGet("check-birthdays")]
    public async Task<IActionResult> CheckBirthdays()
    {
        var userId = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        await _birthdayReminderService.CheckBirthdaysAsync(userId);
        return Ok("Birthday check complete.");
    }
}