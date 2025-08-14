using FluentValidation;
using FluentValidation.AspNetCore;
using InforceTestTask.Application.Features.Urls.Create;
using InforceTestTask.Application.Features.Urls.Delete;
using InforceTestTask.Application.Features.Urls.GetAll;
using InforceTestTask.Application.Features.Urls.GetInfoById;
using InforceTestTask.Application.Features.Users.Login;
using InforceTestTask.Application.Features.Users.Register;
using InforceTestTask.Application.Options;
using InforceTestTask.Application.Services;
using InforceTestTask.Domain.Extensions;
using InforceTestTask.Domain.Interfaces;
using InforceTestTask.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InforceTestTask.Tests.Common;

public static class TestServices
{
    public static IServiceProvider ConfigureTestServices()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())  
            .AddJsonFile("appsettings.json")               
            .Build();

        var services = new ServiceCollection();

        services.AddConfiguredOptions<DbOptions>(configuration);
        services.AddConfiguredOptions<JwtOptions>(configuration);
        services.AddConfiguredOptions<CustomCookieOptions>(configuration);
        
        services.AddDbContext<AppDbContext>(options => 
        {
            options.UseInMemoryDatabase(Guid.NewGuid().ToString())
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
    
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        });
        
        services.AddTransient<IPasswordHasher, PasswordHasher>();
        services.AddTransient<IJwtProvider, JwtProvider>();
        services.AddTransient<IUrlShortener, UrlShortener>();
        
        services.AddValidatorsFromAssembly(typeof(AppDbContext).Assembly);
        services.AddFluentValidationAutoValidation();
        
        services.AddTransient<RegisterUserCommandHandler>();
        services.AddTransient<LoginUserCommandHandler>();
        services.AddTransient<CreateShortUrlCommandHandler>();
        services.AddTransient<DeleteShortUrlCommandHandler>();
        services.AddTransient<GetAllShortUrlsQueryHandler>();
        services.AddTransient<GetShortUrlInfoQueryHandler>();

        var serviceProvider = services.BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        context.Database.EnsureCreated();

        return serviceProvider;
    }
}