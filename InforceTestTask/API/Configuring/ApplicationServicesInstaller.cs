using FluentValidation;
using FluentValidation.AspNetCore;
using InforceTestTask.Application.Features.Users.Register;
using InforceTestTask.Application.Services;
using InforceTestTask.Domain.Interfaces;

namespace InforceTestTask.API.Configuring;

public class ApplicationServicesInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IPasswordHasher, PasswordHasher>();
        services.AddTransient<IJwtProvider, JwtProvider>();
        services.AddTransient<IUrlShortener, UrlShortener>();
        
        services.AddValidatorsFromAssembly(typeof(Program).Assembly);
        services.AddFluentValidationAutoValidation();
        
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(RegisterUserCommandHandler).Assembly);
        });
    }
}