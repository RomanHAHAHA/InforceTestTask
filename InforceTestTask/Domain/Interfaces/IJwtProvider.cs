using InforceTestTask.Domain.Entities;

namespace InforceTestTask.Domain.Interfaces;

public interface IJwtProvider
{
    string GenerateToken(User user);
}