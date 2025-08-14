using System.Security.Claims;
using InforceTestTask.Domain.Models;

namespace InforceTestTask.Domain.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetId(this ClaimsPrincipal? user)
    {
        var userIdClaim = user?.FindFirst(CustomClaims.UserId);

        if (userIdClaim is null)
        {
            return Guid.Empty;
        }

        return Guid.TryParse(userIdClaim.Value, out var userId) ? 
            userId : 
            Guid.Empty;
    }
}