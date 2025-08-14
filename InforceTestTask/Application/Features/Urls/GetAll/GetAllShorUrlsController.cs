using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InforceTestTask.Application.Features.Urls.GetAll;

[Route("api/urls")]
[ApiController]
public class GetAllShorUrlsController(IMediator mediator) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllShorUrlsAsync(CancellationToken cancellation)
    {
        var query = new GetAllShortUrlsQuery();
        var urls = await mediator.Send(query, cancellation);
        return Ok(new { data = urls });
    }
}