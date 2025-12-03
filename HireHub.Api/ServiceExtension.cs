using HireHub.Shared.Authentication;
using HireHub.Shared.Authentication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace HireHub.Api;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterSwaggerGen(
        this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            // Add JWT bearer definition
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });

            // Add global security requirement
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }

    public static IServiceCollection RegisterServices(
        this IServiceCollection services, IConfigurationManager configuration)
    {
        services.AddHireHubAuth(
            configuration.GetSection("JwtSettings").Get<JwtSettings>() ?? 
            throw new InvalidOperationException("JwtNotConfigured")
        );

        services.AddHttpContextAccessor();

        return services;
    }
}
