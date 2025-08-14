using InforceTestTask.Domain.Models;
using InforceTestTask.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InforceTestTask.Application.Features.Urls.GetOriginalUrlByShort;

public class GetOriginalUrlByShortQueryHandler(
    AppDbContext dbContext) : IRequestHandler<GetOriginalUrlByShortQuery, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(
        GetOriginalUrlByShortQuery request, 
        CancellationToken cancellationToken)
    {
        var originalUrl = await dbContext.ShortUrls
            .Where(u => u.ShortCode == request.ShortCode)
            .Select(u => u.OriginalUrl)
            .FirstOrDefaultAsync(cancellationToken);
        
        return string.IsNullOrWhiteSpace(originalUrl) ? 
            ApiResponse<string>.NotFound("Original url not found") :
            ApiResponse<string>.Ok(originalUrl);
    }
}