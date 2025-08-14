using InforceTestTask.Domain.Models;
using InforceTestTask.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InforceTestTask.Application.Features.Urls.GetInfoById;

public class GetShortUrlInfoQueryHandler(
    AppDbContext dbContext) : IRequestHandler<GetShortUrlInfoQuery, ApiResponse<UrlInfoDto>>
{
    public async Task<ApiResponse<UrlInfoDto>> Handle(
        GetShortUrlInfoQuery request, 
        CancellationToken cancellationToken)
    {
        var urlInfo = await dbContext.ShortUrls
            .AsNoTracking()
            .AsSplitQuery()
            .Include(u => u.Content)
            .Include(u => u.Creator)
            .Select(u => new UrlInfoDto
            {
                Id = u.Id,
                OriginalUrl = u.OriginalUrl,
                ShortUrl = $"https://localhost:3000/{u.ShortCode}",
                Content = u.Content!.Description,
                Creator = new CreatorInfoDto
                {
                    Id = u.Creator!.Id,
                    NickName = u.Creator!.NickName,
                    Email = u.Creator!.Email,
                    Role = u.Creator!.Role!.Name,
                    RegisteredDate = $"{u.Creator!.CreatedAt.ToLocalTime():dd.MM.yyyy}"
                }
            })
            .FirstOrDefaultAsync(x => x.Id == request.UrlId, cancellationToken);

        return urlInfo is null ? 
            ApiResponse<UrlInfoDto>.NotFound("Url was not found") :
            ApiResponse<UrlInfoDto>.Ok(urlInfo);
    }
}