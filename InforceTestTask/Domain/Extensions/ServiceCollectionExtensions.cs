using System.Reflection;
using InforceTestTask.API.Configuring;

namespace InforceTestTask.Domain.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddConfiguredOptions<T>(
        this IServiceCollection services, 
        IConfiguration configuration) where T : class
    {
        services.Configure<T>(configuration.GetSection(typeof(T).Name));
    }
    
    public static IServiceCollection InstallServices(
        this IServiceCollection services,
        IConfiguration configuration,
        params Assembly[] assemblies)
    {
        var serviceInstallers = assemblies
            .SelectMany(a => a.DefinedTypes)
            .Where(IsAssignableToType<IServiceInstaller>)
            .Select(Activator.CreateInstance)
            .Cast<IServiceInstaller>();

        foreach (var serviceInstaller in serviceInstallers)
        {
            serviceInstaller.Install(services, configuration);
        }

        return services;

        static bool IsAssignableToType<T>(TypeInfo typeInfo) => 
            typeof(T).IsAssignableFrom(typeInfo) && 
            typeInfo is { IsInterface: false, IsAbstract: false };
    }
}