using InforceTestTask.Domain.Interfaces;
using InforceTestTask.Domain.Models;
using InforceTestTask.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InforceTestTask.Application.Features.Users.Login;

public class LoginUserCommandHandler(
    AppDbContext dbContext,
    IJwtProvider jwtProvider,
    IPasswordHasher passwordHasher) : IRequestHandler<LoginUserCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == request.Dto.Email, cancellationToken);

        if (user is null)
        {
            return ApiResponse<string>.NotFound("User with such email not found");
        }

        if (!passwordHasher.Verify(request.Dto.Password, user.HashedPassword))
        {
            return ApiResponse<string>.Conflict("Incorrect password");
        }

        return jwtProvider.GenerateToken(user);
    }
}