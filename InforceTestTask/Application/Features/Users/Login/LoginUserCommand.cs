using InforceTestTask.Domain.Models;
using MediatR;

namespace InforceTestTask.Application.Features.Users.Login;

public record LoginUserCommand(LoginUserDto Dto) : IRequest<ApiResponse<string>>;