using System.Text.Json;
using InforceTestTask.Application.Options;
using InforceTestTask.Domain.Entities;
using InforceTestTask.Domain.Models;
using InforceTestTask.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace InforceTestTask.Application.Features.AboutPageContent.Update;

public class UpdatePageContentCommandHandler(
    AppDbContext dbContext,
    IOptions<AboutPageOptions> options,
    IWebHostEnvironment env) : IRequestHandler<UpdatePageContentCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(
        UpdatePageContentCommand request, 
        CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == request.CurrentUserId, cancellationToken);

        if (user is null)
        {
            return ApiResponse.NotFound("User not found");
        }

        if (user.RoleId != Role.Admin.Id)
        {
            return ApiResponse.Forbidden("You do not have the admin rights");
        }
        
        try
        {
            var filePath = Path.Combine(env.ContentRootPath, options.Value.FileName);
            var jsonOptions = new JsonSerializerOptions { WriteIndented = true };

            await using var fileStream = File.Create(filePath);
            await JsonSerializer.SerializeAsync(
                fileStream, 
                request.UpdateDto, 
                jsonOptions, 
                cancellationToken);

            return ApiResponse.Ok("Content updated successfully");
        }
        catch (Exception)
        {
            return ApiResponse.InternalServerError();
        }
    }
}