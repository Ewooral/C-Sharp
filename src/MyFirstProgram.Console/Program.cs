using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyFirstProgram.Console.Services;
using MyFirstProgram.Infrastructure.DependencyInjection;

namespace MyFirstProgram.Console;

/// <summary>
/// Main program entry point demonstrating enterprise C# architecture
/// </summary>
internal static class Program
{
    /// <summary>
    /// Application entry point
    /// </summary>
    /// <param name="args">Command line arguments</param>
    /// <returns>Exit code</returns>
    private static async Task<int> Main(string[] args)
    {
        try
        {
            // Create and configure the host
            var host = CreateHostBuilder(args).Build();

            // Run the application
            await host.RunAsync();

            return 0; // Success
        }
        catch (Exception ex)
        {
            // Use console directly since logging might not be configured yet
            System.Console.WriteLine($"Application terminated unexpectedly: {ex.Message}");
            return 1; // Error
        }
    }

    /// <summary>
    /// Creates and configures the host builder with all necessary services
    /// </summary>
    /// <param name="args">Command line arguments</param>
    /// <returns>Configured host builder</returns>
    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                // Configure application configuration sources
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                config.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", 
                    optional: true, reloadOnChange: true);
                config.AddCommandLine(args);
                config.AddEnvironmentVariables();
            })
            .ConfigureServices((context, services) =>
            {
                // Register application services
                services.AddApplicationServices(context.Configuration);
                
                // Validate configuration
                services.ValidateConfiguration(context.Configuration);

                // Register the main application service
                services.AddHostedService<ApplicationHostService>();
            });
}