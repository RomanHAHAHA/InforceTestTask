using InforceTestTask.Domain.Models;
using MediatR;

namespace InforceTestTask.Application.Features.Urls.GetOriginalUrlByShort;

public record GetOriginalUrlByShortQuery(string ShortCode) : IRequest<ApiResponse<string>>;