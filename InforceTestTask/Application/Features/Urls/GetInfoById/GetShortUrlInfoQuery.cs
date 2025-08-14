using InforceTestTask.Domain.Models;
using MediatR;

namespace InforceTestTask.Application.Features.Urls.GetInfoById;

public record GetShortUrlInfoQuery(Guid UrlId) : IRequest<ApiResponse<UrlInfoDto>>;