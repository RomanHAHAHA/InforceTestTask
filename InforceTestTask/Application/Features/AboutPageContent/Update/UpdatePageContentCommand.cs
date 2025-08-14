using InforceTestTask.Domain.Models;
using MediatR;

namespace InforceTestTask.Application.Features.AboutPageContent.Update;

public record UpdatePageContentCommand(
    UpdatePageContentDto UpdateDto,
    Guid CurrentUserId) :  IRequest<ApiResponse>;