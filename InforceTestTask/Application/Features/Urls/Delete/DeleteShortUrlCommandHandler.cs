using InforceTestTask.Domain.Entities;
using InforceTestTask.Domain.Models;
using InforceTestTask.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InforceTestTask.Application.Features.Urls.Delete;

public class DeleteShortUrlCommandHandler(
    AppDbContext dbContext) : IRequestHandler<DeleteShortUrlCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(DeleteShortUrlCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (currentUser is null)
        {
            return ApiResponse.BadRequest("Incorrect user id");
        }
        
        var shortUrl = await dbContext.ShortUrls
            .FirstOrDefaultAsync(su => su.Id == request.UrlId, cancellationToken);

        if (shortUrl is null)
        {
            return ApiResponse.BadRequest("Incorrect URL id");
        }

        if (!CanDeleteAnyUrl(currentUser, shortUrl))
        {
            return ApiResponse.Forbidden("You have no permission to delete other users URLs");
        }
        
        dbContext.ShortUrls.Remove(shortUrl);
        await dbContext.SaveChangesAsync(cancellationToken);
            
        return ApiResponse.Ok();
    }
    
    private static bool CanDeleteAnyUrl(User user, ShortUrl shortUrl)
    {
        return user.RoleId == Role.Admin.Id || 
               (user.RoleId == Role.User.Id && shortUrl.CreatorId == user.Id); 
    }
}