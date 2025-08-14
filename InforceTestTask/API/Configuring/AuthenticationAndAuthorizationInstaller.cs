using System.Text;
using InforceTestTask.Application.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace InforceTestTask.API.Configuring;

public class AuthenticationAndAuthorizationInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = configuration
            .GetSection(nameof(JwtOptions))
            .Get<JwtOptions>() ?? throw new NullReferenceException(nameof(JwtOptions));

        var customCookieOptions = configuration
            .GetSection(nameof(CustomCookieOptions))
            .Get<CustomCookieOptions>() ?? throw new NullReferenceException(nameof(CustomCookieOptions));

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.RequireHttpsMetadata = true;
            options.SaveToken = true;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    context.Token = context.Request.Cookies[customCookieOptions.Name];

                    return Task.CompletedTask;
                }
            };
        });

        services.AddAuthorization();
    }
}