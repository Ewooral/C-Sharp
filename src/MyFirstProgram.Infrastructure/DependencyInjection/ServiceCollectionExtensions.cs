using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyFirstProgram.Application.Services;
using MyFirstProgram.Core.Interfaces;
using Serilog;

namespace MyFirstProgram.Infrastructure.DependencyInjection;

/// <summary>
/// Extension methods for configuring dependency injection
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds all application services to the service collection
    /// </summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register application services
        services.AddScoped<ITypeConversionService, TypeConversionService>();
        services.AddScoped<IOracleService, OracleService>();

        // Configure logging
        services.AddLogging(builder =>
        {
            builder.ClearProviders();
            
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File("logs/myprogram-.log", 
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            builder.AddSerilog(logger);
        });

        return services;
    }

    /// <summary>
    /// Validates the service configuration
    /// </summary>
    public static IServiceCollection ValidateConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        // Validate required configuration sections exist
        var appConfig = configuration.GetSection("Application");
        if (!appConfig.Exists())
        {
            throw new InvalidOperationException("Application configuration section is missing");
        }

        var featuresConfig = configuration.GetSection("Features");
        if (!featuresConfig.Exists())
        {
            throw new InvalidOperationException("Features configuration section is missing");
        }

        return services;
    }
}