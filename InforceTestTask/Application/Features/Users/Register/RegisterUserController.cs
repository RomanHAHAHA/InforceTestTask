using InforceTestTask.Domain.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InforceTestTask.Application.Features.Users.Register;

[Route("api/users")]
[ApiController]
public class RegisterUserController(IMediator mediator) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(
        [FromBody] RegisterUserDto dto,
        CancellationToken cancellationToken)
    {
        var command = new  RegisterUserCommand(dto);
        var response = await mediator.Send(command, cancellationToken);
        return this.HandleResponse(response);
    }
}