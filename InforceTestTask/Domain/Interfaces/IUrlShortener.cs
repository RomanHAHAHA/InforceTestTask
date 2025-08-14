namespace InforceTestTask.Domain.Interfaces;

public interface IUrlShortener
{
    string GenerateShortCode(string originalUrl);
}