using InforceTestTask.Domain.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InforceTestTask.Application.Features.Urls.GetOriginalUrlByShort;

[Route("api/urls")]
[ApiController]
public class GetOriginalUrlByShortController(IMediator mediator) : ControllerBase
{
    [HttpGet("{shortCode}")]
    public async Task<IActionResult> GetOriginalUrlByShortAsync(
        string shortCode,
        CancellationToken cancellationToken)
    {
        var query = new GetOriginalUrlByShortQuery(shortCode);
        var response = await mediator.Send(query, cancellationToken);
        return this.HandleResponse(response);
    }
}