using InforceTestTask.Domain.Models;
using MediatR;

namespace InforceTestTask.Application.Features.Users.Register;

public record RegisterUserCommand(RegisterUserDto Dto) : IRequest<ApiResponse>;