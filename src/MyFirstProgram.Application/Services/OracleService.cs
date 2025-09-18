using Microsoft.Extensions.Logging;
using MyFirstProgram.Core.Interfaces;
using MyFirstProgram.Core.Models;

namespace MyFirstProgram.Application.Services;

/// <summary>
/// Implementation of oracle service (Magic 8-Ball style responses)
/// </summary>
public sealed class OracleService : IOracleService
{
    private readonly ILogger<OracleService> _logger;
    private readonly Random _random;

    private static readonly IReadOnlyList<string> Answers = new[]
    {
        "It is certain.",
        "Reply hazy, try again.",
        "Don't count on it.",
        "It is decidedly so.",
        "Ask again later.",
        "My reply is no.",
        "Without a doubt.",
        "Better not tell you now.",
        "My sources say no.",
        "Yes – definitely.",
        "Cannot predict now.",
        "Outlook not so good.",
        "You may rely on it.",
        "Concentrate and ask again.",
        "Very doubtful.",
        "As I see it, yes.",
        "Most likely.",
        "Outlook good.",
        "Yes.",
        "Signs point to yes."
    };

    public OracleService(ILogger<OracleService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _random = new Random();
    }

    public Task<OracleResponse> GetResponseAsync(string question)
    {
        if (string.IsNullOrWhiteSpace(question))
        {
            throw new ArgumentException("Question cannot be null or empty", nameof(question));
        }

        _logger.LogInformation("Generating oracle response for question: {Question}", question);

        var answerIndex = _random.Next(0, Answers.Count);
        var selectedAnswer = Answers[answerIndex];

        var response = new OracleResponse
        {
            Question = question.Trim(),
            Answer = selectedAnswer,
            ResponseTime = DateTime.UtcNow
        };

        _logger.LogDebug("Selected answer: {Answer}", selectedAnswer);

        return Task.FromResult(response);
    }

    public IReadOnlyCollection<string> GetAllPossibleAnswers()
    {
        return Answers;
    }
}