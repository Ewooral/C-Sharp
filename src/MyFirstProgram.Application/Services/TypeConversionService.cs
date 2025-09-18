using Microsoft.Extensions.Logging;
using MyFirstProgram.Core.Interfaces;
using MyFirstProgram.Core.Models;

namespace MyFirstProgram.Application.Services;

/// <summary>
/// Implementation of type conversion service with proper error handling and logging
/// </summary>
public sealed class TypeConversionService : ITypeConversionService
{
    private readonly ILogger<TypeConversionService> _logger;

    public TypeConversionService(ILogger<TypeConversionService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public TypeConversionResult ConvertFloatToInt(float value)
    {
        try
        {
            _logger.LogDebug("Converting float value {Value} to integer", value);

            // Check for overflow conditions
            if (value > int.MaxValue)
            {
                var errorMsg = $"Value {value} exceeds maximum integer value ({int.MaxValue})";
                _logger.LogWarning(errorMsg);
                return TypeConversionResult.Failure(value, errorMsg);
            }

            if (value < int.MinValue)
            {
                var errorMsg = $"Value {value} is below minimum integer value ({int.MinValue})";
                _logger.LogWarning(errorMsg);
                return TypeConversionResult.Failure(value, errorMsg);
            }

            // Perform explicit conversion
            var convertedValue = (int)value;
            
            _logger.LogInformation("Successfully converted {OriginalValue} to {ConvertedValue}", 
                value, convertedValue);

            return TypeConversionResult.Success(value, convertedValue);
        }
        catch (Exception ex)
        {
            var errorMsg = $"Unexpected error during conversion: {ex.Message}";
            _logger.LogError(ex, errorMsg);
            return TypeConversionResult.Failure(value, errorMsg);
        }
    }
}