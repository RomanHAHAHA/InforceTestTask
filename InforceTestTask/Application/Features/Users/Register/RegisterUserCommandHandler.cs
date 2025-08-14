using InforceTestTask.Domain.Entities;
using InforceTestTask.Domain.Interfaces;
using InforceTestTask.Domain.Models;
using InforceTestTask.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InforceTestTask.Application.Features.Users.Register;

public class RegisterUserCommandHandler(
    AppDbContext dbContext,
    IPasswordHasher passwordHasher) : IRequestHandler<RegisterUserCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var userExist = await dbContext.Users
            .AnyAsync(u => u.Email == request.Dto.Email, cancellationToken);

        if (userExist)
        {
            return ApiResponse.Conflict("User with the same email already exists.");
        }
        
        var user = new User
        {
            NickName = request.Dto.NickName,
            Email = request.Dto.Email,
            HashedPassword = passwordHasher.HashPassword(request.Dto.Password),
            RoleId = request.Dto.IsAdmin ? Role.Admin.Id : Role.User.Id
        };
        
        await dbContext.Users.AddAsync(user, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
            
        return ApiResponse.Ok();
    }
}