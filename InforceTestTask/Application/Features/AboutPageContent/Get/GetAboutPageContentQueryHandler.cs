using System.Text.Json;
using InforceTestTask.Application.Options;
using InforceTestTask.Domain.Models;
using MediatR;
using Microsoft.Extensions.Options;

namespace InforceTestTask.Application.Features.AboutPageContent.Get;

public class GetAboutPageContentQueryHandler(
    IOptions<AboutPageOptions> options,
    IWebHostEnvironment env) 
    : IRequestHandler<GetAboutPageContentQuery, ApiResponse<AboutPageDto>>
{
    public async Task<ApiResponse<AboutPageDto>> Handle(
        GetAboutPageContentQuery request, 
        CancellationToken cancellationToken)
    {
        var filePath = Path.Combine(env.ContentRootPath, options.Value.FileName);

        if (!File.Exists(filePath))
        {
            return ApiResponse<AboutPageDto>.NotFound("File not found");
        }

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true, 
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true
        };

        try
        {
            await using var fileStream = File.OpenRead(filePath);
            var content = await JsonSerializer.DeserializeAsync<AboutPageDto>(
                fileStream, 
                jsonOptions, 
                cancellationToken);

            return content == null 
                ? ApiResponse<AboutPageDto>.BadRequest("Invalid file content") 
                : ApiResponse<AboutPageDto>.Ok(content);
        }
        catch (JsonException ex)
        {
            return ApiResponse<AboutPageDto>.BadRequest($"JSON error: {ex.Message}");
        }
    }
}