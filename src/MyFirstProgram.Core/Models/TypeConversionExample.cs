namespace MyFirstProgram.Core.Models;

/// <summary>
/// Represents a numeric conversion operation result
/// </summary>
public sealed record TypeConversionResult
{
    public float OriginalValue { get; init; }
    public int ConvertedValue { get; init; }
    public bool IsSuccessful { get; init; }
    public string? ErrorMessage { get; init; }

    public static TypeConversionResult Success(float originalValue, int convertedValue)
        => new() 
        { 
            OriginalValue = originalValue, 
            ConvertedValue = convertedValue, 
            IsSuccessful = true 
        };

    public static TypeConversionResult Failure(float originalValue, string errorMessage)
        => new() 
        { 
            OriginalValue = originalValue, 
            IsSuccessful = false, 
            ErrorMessage = errorMessage 
        };
}

/// <summary>
/// Represents a Magic 8-Ball oracle response
/// </summary>
public sealed record OracleResponse
{
    public string Question { get; init; } = string.Empty;
    public string Answer { get; init; } = string.Empty;
    public DateTime ResponseTime { get; init; } = DateTime.UtcNow;
}