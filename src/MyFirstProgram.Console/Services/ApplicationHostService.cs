using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyFirstProgram.Core.Interfaces;

namespace MyFirstProgram.Console.Services;

/// <summary>
/// Main application host service that orchestrates the program flow
/// </summary>
public sealed class ApplicationHostService : BackgroundService
{
    private readonly ILogger<ApplicationHostService> _logger;
    private readonly IConfiguration _configuration;
    private readonly ITypeConversionService _typeConversionService;
    private readonly IOracleService _oracleService;
    private readonly IHostApplicationLifetime _applicationLifetime;

    public ApplicationHostService(
        ILogger<ApplicationHostService> logger,
        IConfiguration configuration,
        ITypeConversionService typeConversionService,
        IOracleService oracleService,
        IHostApplicationLifetime applicationLifetime)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _typeConversionService = typeConversionService ?? throw new ArgumentNullException(nameof(typeConversionService));
        _oracleService = oracleService ?? throw new ArgumentNullException(nameof(oracleService));
        _applicationLifetime = applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("Starting MyFirstProgram application...");

            await RunApplicationAsync(stoppingToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while running the application");
        }
        finally
        {
            _applicationLifetime.StopApplication();
        }
    }

    private async Task RunApplicationAsync(CancellationToken cancellationToken)
    {
        // Display welcome message
        var appName = _configuration.GetValue<string>("Application:Name") ?? "MyFirstProgram";
        var appVersion = _configuration.GetValue<string>("Application:Version") ?? "1.0.0";
        
        System.Console.WriteLine($"Welcome to {appName} v{appVersion}!");
        System.Console.WriteLine("This demonstrates enterprise C# programming practices.");
        System.Console.WriteLine(new string('-', 50));

        // Demonstrate type conversion
        if (_configuration.GetValue<bool>("Features:EnableTypeConversion"))
        {
            await DemonstrateTypeConversion();
        }

        // Demonstrate oracle service
        if (_configuration.GetValue<bool>("Features:EnableOracleService"))
        {
            await DemonstrateOracleService();
        }

        System.Console.WriteLine(new string('-', 50));
        System.Console.WriteLine("Press any key to exit...");
        System.Console.ReadKey();
    }

    private async Task DemonstrateTypeConversion()
    {
        System.Console.WriteLine("\n=== Type Conversion Demonstration ===");
        
        const float testValue = 50.75f;
        var result = _typeConversionService.ConvertFloatToInt(testValue);

        if (result.IsSuccessful)
        {
            System.Console.WriteLine($"Successfully converted {result.OriginalValue} to {result.ConvertedValue}");
        }
        else
        {
            System.Console.WriteLine($"Conversion failed: {result.ErrorMessage}");
        }

        await Task.CompletedTask;
    }

    private async Task DemonstrateOracleService()
    {
        System.Console.WriteLine("\n=== Magic 8-Ball Oracle Demonstration ===");
        
        const string question = "Will this application be successful?";
        var response = await _oracleService.GetResponseAsync(question);

        System.Console.WriteLine($"Question: {response.Question}");
        System.Console.WriteLine($"Oracle says: {response.Answer}");
        System.Console.WriteLine($"Response time: {response.ResponseTime:yyyy-MM-dd HH:mm:ss} UTC");

        System.Console.WriteLine($"\nTotal possible answers: {_oracleService.GetAllPossibleAnswers().Count}");
    }
}