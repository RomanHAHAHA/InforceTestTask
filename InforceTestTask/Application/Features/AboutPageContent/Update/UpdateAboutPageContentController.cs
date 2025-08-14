using InforceTestTask.Domain.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InforceTestTask.Application.Features.AboutPageContent.Update;

[Route("api/about-page")]
[ApiController]
public class UpdateAboutPageContentController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateAboutPageContentAsync(
        [FromBody] UpdatePageContentDto updateDto,
        CancellationToken cancellationToken)
    {
        var command = new UpdatePageContentCommand(updateDto, User.GetId());
        var response = await mediator.Send(command, cancellationToken);
        return this.HandleResponse(response);
    }
}