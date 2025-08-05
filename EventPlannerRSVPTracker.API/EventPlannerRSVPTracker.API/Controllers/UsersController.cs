using EventPlannerRSVPTracker.App.Abstractions.Services;
using EventPlannerRSVPTracker.App.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EventPlannerRSVPTracker.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly IUsersService _userService;

    public UsersController(ILogger<UsersController> logger, IUsersService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login([FromBody] string username, CancellationToken cancellationToken = default)
    {

        var result = await _userService.Login(username, cancellationToken);

        if (result.IsSuccess)
        {
            var jsonResponse = JsonResponse.Success($"[{username}] logged in successfully", result.Data);

            return Ok(jsonResponse);
        }
        else
        {
            var jsonResponse = JsonResponse.Fail(result.ErrorDetails![0], result.ErrorDetails![0].Message);

            return Ok(jsonResponse);
        }
    }
}
