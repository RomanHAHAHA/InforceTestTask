using InforceTestTask.Application.Options;
using InforceTestTask.Domain.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace InforceTestTask.Application.Features.Users.Login;

[Route("api/users")]
[ApiController]
public class LoginUserController(
    IMediator mediator,
    IOptions<CustomCookieOptions> options) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(
        [FromBody] LoginUserDto loginDto,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new LoginUserCommand(loginDto), cancellationToken);
        
        if (response.IsFailure)
        {
            return this.HandleResponse(response);
        }

        var token = response.Data;
        HttpContext.Response.Cookies.Append(options.Value.Name, token);

        return Ok(new { token });
    }
}