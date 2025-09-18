using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using MyFirstProgram.Application.Services;
using Xunit;

namespace MyFirstProgram.UnitTests.Services;

/// <summary>
/// Unit tests for TypeConversionService
/// </summary>
public sealed class TypeConversionServiceTests
{
    private readonly Mock<ILogger<TypeConversionService>> _mockLogger;
    private readonly TypeConversionService _service;

    public TypeConversionServiceTests()
    {
        _mockLogger = new Mock<ILogger<TypeConversionService>>();
        _service = new TypeConversionService(_mockLogger.Object);
    }

    [Fact]
    public void ConvertFloatToInt_WithValidValue_ShouldReturnSuccess()
    {
        // Arrange
        const float testValue = 42.7f;
        const int expectedResult = 42;

        // Act
        var result = _service.ConvertFloatToInt(testValue);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccessful.Should().BeTrue();
        result.OriginalValue.Should().Be(testValue);
        result.ConvertedValue.Should().Be(expectedResult);
        result.ErrorMessage.Should().BeNull();
    }

    [Fact]
    public void ConvertFloatToInt_WithZero_ShouldReturnSuccess()
    {
        // Arrange
        const float testValue = 0.0f;
        const int expectedResult = 0;

        // Act
        var result = _service.ConvertFloatToInt(testValue);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccessful.Should().BeTrue();
        result.OriginalValue.Should().Be(testValue);
        result.ConvertedValue.Should().Be(expectedResult);
    }

    [Fact]
    public void ConvertFloatToInt_WithNegativeValue_ShouldReturnSuccess()
    {
        // Arrange
        const float testValue = -123.456f;
        const int expectedResult = -123;

        // Act
        var result = _service.ConvertFloatToInt(testValue);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccessful.Should().BeTrue();
        result.OriginalValue.Should().Be(testValue);
        result.ConvertedValue.Should().Be(expectedResult);
    }

    [Fact]
    public void ConvertFloatToInt_WithMaxIntValue_ShouldReturnSuccess()
    {
        // Arrange
        var testValue = (float)int.MaxValue;

        // Act
        var result = _service.ConvertFloatToInt(testValue);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccessful.Should().BeTrue();
        result.OriginalValue.Should().Be(testValue);
        result.ConvertedValue.Should().Be(int.MaxValue);
    }

    [Fact]
    public void ConvertFloatToInt_WithValueExceedingMaxInt_ShouldReturnFailure()
    {
        // Arrange
        var testValue = (float)int.MaxValue + 1000f;

        // Act
        var result = _service.ConvertFloatToInt(testValue);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccessful.Should().BeFalse();
        result.OriginalValue.Should().Be(testValue);
        result.ErrorMessage.Should().NotBeNullOrEmpty();
        result.ErrorMessage.Should().Contain("exceeds maximum integer value");
    }

    [Fact]
    public void ConvertFloatToInt_WithValueBelowMinInt_ShouldReturnFailure()
    {
        // Arrange
        var testValue = (float)int.MinValue - 1000f;

        // Act
        var result = _service.ConvertFloatToInt(testValue);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccessful.Should().BeFalse();
        result.OriginalValue.Should().Be(testValue);
        result.ErrorMessage.Should().NotBeNullOrEmpty();
        result.ErrorMessage.Should().Contain("below minimum integer value");
    }

    [Theory]
    [InlineData(1.1f, 1)]
    [InlineData(99.9f, 99)]
    [InlineData(-1.9f, -1)]
    [InlineData(0.5f, 0)]
    public void ConvertFloatToInt_WithVariousValues_ShouldTruncateCorrectly(float input, int expected)
    {
        // Act
        var result = _service.ConvertFloatToInt(input);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccessful.Should().BeTrue();
        result.ConvertedValue.Should().Be(expected);
    }

    [Fact]
    public void Constructor_WithNullLogger_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Action act = () => new TypeConversionService(null!);
        act.Should().Throw<ArgumentNullException>().WithParameterName("logger");
    }
}