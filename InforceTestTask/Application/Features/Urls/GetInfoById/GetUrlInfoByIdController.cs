using InforceTestTask.Domain.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InforceTestTask.Application.Features.Urls.GetInfoById;

[Route("api/urls")]
[ApiController]
public class GetUrlInfoByIdController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpGet("{urlId:guid}")]
    public async Task<IActionResult> GetUrlInfoByIdAsync(Guid urlId, CancellationToken cancellationToken)
    {
        var query =  new GetShortUrlInfoQuery(urlId);
        var response = await mediator.Send(query, cancellationToken);
        return this.HandleResponse(response);
    }
}