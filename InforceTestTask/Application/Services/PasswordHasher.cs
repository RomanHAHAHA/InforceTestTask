using System.Security.Cryptography;
using System.Text;
using InforceTestTask.Domain.Interfaces;

namespace InforceTestTask.Application.Services;

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        var hashedBytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(hashedBytes).ToLowerInvariant();
    }

    public bool Verify(string password, string hashedPassword)
        => HashPassword(password).Equals(hashedPassword);
}