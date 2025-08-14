using InforceTestTask.Application.Options;
using InforceTestTask.Persistence.Contexts;

namespace InforceTestTask.API.Configuring;

public class DatabaseInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        var dbOptions = configuration
            .GetSection(nameof(DbOptions))
            .Get<DbOptions>() ?? throw new NullReferenceException(nameof(DbOptions));

        services.AddSqlServer<AppDbContext>(dbOptions.ConnectionString);
    }
}