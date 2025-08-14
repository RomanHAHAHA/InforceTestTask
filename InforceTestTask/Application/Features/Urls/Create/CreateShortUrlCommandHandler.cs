using InforceTestTask.Domain.Entities;
using InforceTestTask.Domain.Interfaces;
using InforceTestTask.Domain.Models;
using InforceTestTask.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InforceTestTask.Application.Features.Urls.Create;

public class CreateShortUrlCommandHandler(
    AppDbContext dbContext,
    IUrlShortener urlShortener) : IRequestHandler<CreateShortUrlCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(CreateShortUrlCommand request, CancellationToken cancellationToken)
    {
        var urlExist = await dbContext.ShortUrls
            .AnyAsync(u => u.OriginalUrl == request.CreateDto.Url, cancellationToken);
        
        if (urlExist)
        {
            return ApiResponse.BadRequest("URL already exists");
        }

        var shortUrl = new ShortUrl
        {
            OriginalUrl = request.CreateDto.Url,
            ShortCode = urlShortener.GenerateShortCode(request.CreateDto.Url),
            CreatorId = request.CurrentUserId,
            Content = new UrlContent
            {
                Description = request.CreateDto.Description,
            }
        };

        await dbContext.ShortUrls.AddAsync(shortUrl, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ApiResponse.Ok();
    }
}