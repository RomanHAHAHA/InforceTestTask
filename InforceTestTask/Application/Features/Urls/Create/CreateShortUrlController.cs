using InforceTestTask.Domain.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InforceTestTask.Application.Features.Urls.Create;

[Route("api/urls")]
[ApiController]
public class CreateShortUrlController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateUrlAsync(
        [FromBody] CreateShortUrlDto dto,
        CancellationToken cancellationToken)
    {
        var command = new CreateShortUrlCommand(dto, User.GetId());
        var response = await mediator.Send(command, cancellationToken);
        return this.HandleResponse(response);
    }
}