using InforceTestTask.Domain.Models;
using MediatR;

namespace InforceTestTask.Application.Features.Urls.Create;

public record CreateShortUrlCommand(CreateShortUrlDto CreateDto, Guid CurrentUserId) : IRequest<ApiResponse>;