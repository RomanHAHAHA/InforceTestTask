using InforceTestTask.Domain.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InforceTestTask.Application.Features.Urls.Delete;

[Route("api/urls")]
[ApiController]
public class DeleteShortUrlController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpDelete("{urlId:guid}")]
    public async Task<IActionResult> DeleteShortUrlAsync(Guid urlId, CancellationToken cancellationToken)
    {
        var command = new DeleteShortUrlCommand(urlId, User.GetId());
        var response = await mediator.Send(command, cancellationToken);
        return this.HandleResponse(response);
    }
}