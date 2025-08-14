namespace InforceTestTask.API.Configuring;

public interface IServiceInstaller
{
    void Install(IServiceCollection services, IConfiguration configuration);
}