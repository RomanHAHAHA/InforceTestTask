using InforceTestTask.Application.Features.AboutPageContent.Get;
using InforceTestTask.Application.Options;
using InforceTestTask.Domain.Extensions;

namespace InforceTestTask.API.Configuring;

public class OptionsInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddConfiguredOptions<DbOptions>(configuration);
        services.AddConfiguredOptions<JwtOptions>(configuration);
        services.AddConfiguredOptions<CustomCookieOptions>(configuration);
        services.AddConfiguredOptions<AboutPageOptions>(configuration);
    }
}