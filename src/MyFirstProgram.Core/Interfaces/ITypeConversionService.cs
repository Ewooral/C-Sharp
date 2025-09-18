using MyFirstProgram.Core.Models;

namespace MyFirstProgram.Core.Interfaces;

/// <summary>
/// Service for handling type conversion operations
/// </summary>
public interface ITypeConversionService
{
    /// <summary>
    /// Converts a float to integer with proper error handling
    /// </summary>
    /// <param name="value">The float value to convert</param>
    /// <returns>Result of the conversion operation</returns>
    TypeConversionResult ConvertFloatToInt(float value);
}

/// <summary>
/// Service for generating oracle (Magic 8-Ball) responses
/// </summary>
public interface IOracleService
{
    /// <summary>
    /// Generates a random oracle response for a given question
    /// </summary>
    /// <param name="question">The question to ask the oracle</param>
    /// <returns>Oracle response with answer</returns>
    Task<OracleResponse> GetResponseAsync(string question);

    /// <summary>
    /// Gets all possible oracle answers
    /// </summary>
    /// <returns>Collection of all possible answers</returns>
    IReadOnlyCollection<string> GetAllPossibleAnswers();
}