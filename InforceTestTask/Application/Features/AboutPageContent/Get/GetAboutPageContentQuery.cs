using InforceTestTask.Domain.Models;
using MediatR;

namespace InforceTestTask.Application.Features.AboutPageContent.Get;

public record GetAboutPageContentQuery : IRequest<ApiResponse<AboutPageDto>>;