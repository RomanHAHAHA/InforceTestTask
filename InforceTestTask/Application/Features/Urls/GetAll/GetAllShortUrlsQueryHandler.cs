using InforceTestTask.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InforceTestTask.Application.Features.Urls.GetAll;

public class GetAllShortUrlsQueryHandler(
    AppDbContext dbContext) : IRequestHandler<GetAllShortUrlsQuery, List<ShortUrlTableDto>>
{
    public async Task<List<ShortUrlTableDto>> Handle(
        GetAllShortUrlsQuery request, 
        CancellationToken cancellationToken)
    {
        return await dbContext.ShortUrls
            .AsNoTracking()
            .Select(su => new ShortUrlTableDto
            {
                Id = su.Id,
                CreatorId = su.CreatorId,
                OriginalUrl = su.OriginalUrl,
                ShortUrl = $"https://localhost:3000/{su.ShortCode}",
                CreatedAt = $"{su.CreatedAt.ToLocalTime():dd.MM.yyyy HH:mm}",
            })
            .ToListAsync(cancellationToken);
    }
}