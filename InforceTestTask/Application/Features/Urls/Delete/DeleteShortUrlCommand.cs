using InforceTestTask.Domain.Models;
using MediatR;

namespace InforceTestTask.Application.Features.Urls.Delete;

public record DeleteShortUrlCommand(Guid UrlId, Guid UserId) : IRequest<ApiResponse>;