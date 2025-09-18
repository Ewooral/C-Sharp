using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using MyFirstProgram.Application.Services;
using Xunit;

namespace MyFirstProgram.UnitTests.Services;

/// <summary>
/// Unit tests for OracleService
/// </summary>
public sealed class OracleServiceTests
{
    private readonly Mock<ILogger<OracleService>> _mockLogger;
    private readonly OracleService _service;

    public OracleServiceTests()
    {
        _mockLogger = new Mock<ILogger<OracleService>>();
        _service = new OracleService(_mockLogger.Object);
    }

    [Fact]
    public async Task GetResponseAsync_WithValidQuestion_ShouldReturnResponse()
    {
        // Arrange
        const string question = "Will this test pass?";

        // Act
        var result = await _service.GetResponseAsync(question);

        // Assert
        result.Should().NotBeNull();
        result.Question.Should().Be(question);
        result.Answer.Should().NotBeNullOrEmpty();
        result.ResponseTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        
        var allAnswers = _service.GetAllPossibleAnswers();
        allAnswers.Should().Contain(result.Answer);
    }

    [Fact]
    public async Task GetResponseAsync_WithQuestionWithWhitespace_ShouldTrimQuestion()
    {
        // Arrange
        const string questionWithSpaces = "  Will this work?  ";
        const string expectedQuestion = "Will this work?";

        // Act
        var result = await _service.GetResponseAsync(questionWithSpaces);

        // Assert
        result.Should().NotBeNull();
        result.Question.Should().Be(expectedQuestion);
        result.Answer.Should().NotBeNullOrEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public async Task GetResponseAsync_WithInvalidQuestion_ShouldThrowArgumentException(string invalidQuestion)
    {
        // Act & Assert
        var act = async () => await _service.GetResponseAsync(invalidQuestion);
        await act.Should().ThrowAsync<ArgumentException>()
            .WithParameterName("question")
            .WithMessage("Question cannot be null or empty*");
    }

    [Fact]
    public async Task GetResponseAsync_WithNullQuestion_ShouldThrowArgumentException()
    {
        // Act & Assert
        var act = async () => await _service.GetResponseAsync(null!);
        await act.Should().ThrowAsync<ArgumentException>()
            .WithParameterName("question")
            .WithMessage("Question cannot be null or empty*");
    }

    [Fact]
    public void GetAllPossibleAnswers_ShouldReturnExpectedAnswers()
    {
        // Arrange
        var expectedAnswers = new[]
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

        // Act
        var result = _service.GetAllPossibleAnswers();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(20);
        result.Should().BeEquivalentTo(expectedAnswers);
    }

    [Fact]
    public async Task GetResponseAsync_CallMultipleTimes_ShouldReturnDifferentAnswers()
    {
        // Arrange
        const string question = "Test question";
        const int numberOfCalls = 50;
        var responses = new HashSet<string>();

        // Act
        for (var i = 0; i < numberOfCalls; i++)
        {
            var response = await _service.GetResponseAsync(question);
            responses.Add(response.Answer);
        }

        // Assert
        // Given the randomness, we expect to see multiple different answers
        // With 50 calls and 20 possible answers, we should see variety
        responses.Should().HaveCountGreaterThan(1, "random selection should produce variety over multiple calls");
    }

    [Fact]
    public async Task GetResponseAsync_ShouldAlwaysReturnValidAnswer()
    {
        // Arrange
        const string question = "Test question";
        const int numberOfTests = 10;
        var allValidAnswers = _service.GetAllPossibleAnswers();

        // Act & Assert
        for (var i = 0; i < numberOfTests; i++)
        {
            var response = await _service.GetResponseAsync(question);
            allValidAnswers.Should().Contain(response.Answer, 
                "returned answer should always be one of the predefined valid answers");
        }
    }

    [Fact]
    public void Constructor_WithNullLogger_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Action act = () => new OracleService(null!);
        act.Should().Throw<ArgumentNullException>().WithParameterName("logger");
    }

    [Fact]
    public async Task GetResponseAsync_ShouldLogInformation()
    {
        // Arrange
        const string question = "Test logging";

        // Act
        await _service.GetResponseAsync(question);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Generating oracle response for question")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact] 
    public void GetAllPossibleAnswers_ShouldReturnReadOnlyCollection()
    {
        // Act
        var result = _service.GetAllPossibleAnswers();

        // Assert
        result.Should().BeAssignableTo<IReadOnlyCollection<string>>();
        // Verify it's truly read-only by checking the type
        result.GetType().Should().Implement<IReadOnlyCollection<string>>();
    }
}