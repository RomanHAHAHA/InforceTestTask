namespace InforceTestTask.API.Configuring;

public class CorsInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins("https://localhost:3000")
                    .AllowCredentials()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
    }
}