using System.Security.Cryptography;
using System.Text;
using InforceTestTask.Domain.Interfaces;

namespace InforceTestTask.Application.Services;

public class UrlShortener : IUrlShortener
{
    public string GenerateShortCode(string originalUrl)
    {
        var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(originalUrl + Guid.NewGuid() + DateTime.Now.Ticks));
    
        return Convert.ToBase64String(hashBytes, 0, 6)
            .Replace("/", "_")
            .Replace("+", "-")
            .TrimEnd('=');
    }
}