using InforceTestTask.Domain.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InforceTestTask.Application.Features.AboutPageContent.Get;

[Route("api/about-page")]
[ApiController]
public class GetAboutPageContentController(IMediator mediator) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("content")]
    public async Task<IActionResult> GetAboutPageContent(CancellationToken cancellationToken)
    {
        var query = new GetAboutPageContentQuery();
        var response = await mediator.Send(query, cancellationToken);
        return this.HandleResponse(response);
    }
}