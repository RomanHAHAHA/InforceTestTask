using InforceTestTask.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InforceTestTask.Application.Features.Users.GetClaims;

[Route("/api/users")]
[ApiController]
public class GetUserClaimsController : Controller
{
    [Authorize]
    [HttpGet("get-claims")]
    public IActionResult GetUserClaimsData()
    {
        var userCookiesData = new
        {
            UserId = User.FindFirst(CustomClaims.UserId)!.Value,
            NickName = User.FindFirst(CustomClaims.NickName)!.Value,
            Role = User.FindFirst(CustomClaims.Role)!.Value,
        };

        return Ok(new { userCookiesData });
    }
}