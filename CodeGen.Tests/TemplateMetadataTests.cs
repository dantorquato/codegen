using CodeGen.Models;

namespace CodeGen.Tests;

public class TemplateMetadataTests
{
    [Fact]
    public void HasAnyTag_ShouldReturnTrue_WhenTagMatches()
    {
        // Arrange
        var metadata = new TemplateMetadata
        {
            Tags = new List<string> { "entity", "model", "domain" }
        };
        var requestedTags = new List<string> { "entity" };

        // Act
        var result = metadata.HasAnyTag(requestedTags);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasAnyTag_ShouldReturnTrue_WhenAnyTagMatches()
    {
        // Arrange
        var metadata = new TemplateMetadata
        {
            Tags = new List<string> { "entity", "model", "domain" }
        };
        var requestedTags = new List<string> { "controller", "domain" };

        // Act
        var result = metadata.HasAnyTag(requestedTags);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasAnyTag_ShouldReturnFalse_WhenNoTagMatches()
    {
        // Arrange
        var metadata = new TemplateMetadata
        {
            Tags = new List<string> { "entity", "model", "domain" }
        };
        var requestedTags = new List<string> { "controller", "service" };

        // Act
        var result = metadata.HasAnyTag(requestedTags);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasAnyTag_ShouldReturnFalse_WhenTemplateHasNoTags()
    {
        // Arrange
        var metadata = new TemplateMetadata
        {
            Tags = new List<string>()
        };
        var requestedTags = new List<string> { "entity" };

        // Act
        var result = metadata.HasAnyTag(requestedTags);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasAnyTag_ShouldBeCaseInsensitive()
    {
        // Arrange
        var metadata = new TemplateMetadata
        {
            Tags = new List<string> { "entity", "Model", "DOMAIN" }
        };
        var requestedTags = new List<string> { "ENTITY", "model" };

        // Act
        var result1 = metadata.HasAnyTag(new[] { "ENTITY" });
        var result2 = metadata.HasAnyTag(new[] { "model" });

        // Assert
        Assert.True(result1);
        Assert.True(result2);
    }

    [Fact]
    public void HasAnyTag_ShouldReturnTrue_WithMultipleMatchingTags()
    {
        // Arrange
        var metadata = new TemplateMetadata
        {
            Tags = new List<string> { "entity", "model", "controller", "service" }
        };
        var requestedTags = new List<string> { "entity", "controller" };

        // Act
        var result = metadata.HasAnyTag(requestedTags);

        // Assert
        Assert.True(result);
    }
}
