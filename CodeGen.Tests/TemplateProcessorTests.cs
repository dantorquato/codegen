using CodeGen.Services;

namespace CodeGen.Tests;

public class TemplateProcessorTests : IDisposable
{
    private readonly TemplateProcessor _processor = new();
    private readonly string _tempDir = Path.Combine(Path.GetTempPath(), $"codegen-tests-{Guid.NewGuid()}");

    public TemplateProcessorTests()
    {
        Directory.CreateDirectory(_tempDir);
    }

    [Fact]
    public void ProcessTemplateFile_ShouldExtractMetadata()
    {
        // Arrange
        var templatePath = CreateTempTemplate(@"
// META: output=generated/Models/{{EntityName}}.cs
// META: description=Model class

namespace Test;

public class {{EntityName}} {}
");

        // Act
        var template = _processor.ProcessTemplateFile(templatePath);

        // Assert
        Assert.Equal("generated/Models/{{EntityName}}.cs", template.Metadata.Output);
        Assert.Equal("Model class", template.Metadata.Description);
    }

    [Fact]
    public void ProcessTemplateFile_ShouldRemoveMetadataFromContent()
    {
        // Arrange
        var templatePath = CreateTempTemplate(@"
// META: output=test/test.cs

namespace Test;

public class Test {}
");

        // Act
        var template = _processor.ProcessTemplateFile(templatePath);

        // Assert
        Assert.DoesNotContain("// META:", template.Content);
        Assert.Contains("namespace Test;", template.Content);
        Assert.Contains("public class Test {}", template.Content);
    }

    [Fact]
    public void ProcessTemplateFile_ShouldThrowWhenFileNotFound()
    {
        // Arrange
        var nonExistentPath = Path.Combine(_tempDir, "nonexistent.template.cs");

        // Act & Assert
        Assert.Throws<FileNotFoundException>(() => _processor.ProcessTemplateFile(nonExistentPath));
    }

    [Fact]
    public void ProcessTemplateFile_ShouldHandleEmptyMetadata()
    {
        // Arrange
        var templatePath = CreateTempTemplate(@"
namespace Test;

public class Test {}
");

        // Act
        var template = _processor.ProcessTemplateFile(templatePath);

        // Assert
        Assert.Null(template.Metadata.Output);
        Assert.Null(template.Metadata.Description);
        Assert.False(template.Metadata.IsValid);
    }

    [Fact]
    public void ProcessTemplateFile_ShouldExtractTags()
    {
        // Arrange
        var templatePath = CreateTempTemplate(@"
// META: output=generated/{{EntityName}}.cs
// META: tags=entity, model, domain

namespace Test;

public class {{EntityName}} {}
");

        // Act
        var template = _processor.ProcessTemplateFile(templatePath);

        // Assert
        Assert.Equal(3, template.Metadata.Tags.Count);
        Assert.Contains("entity", template.Metadata.Tags);
        Assert.Contains("model", template.Metadata.Tags);
        Assert.Contains("domain", template.Metadata.Tags);
    }

    [Fact]
    public void ProcessTemplateFile_ShouldHandleEmptyTags()
    {
        // Arrange
        var templatePath = CreateTempTemplate(@"
// META: output=generated/{{EntityName}}.cs

namespace Test;

public class {{EntityName}} {}
");

        // Act
        var template = _processor.ProcessTemplateFile(templatePath);

        // Assert
        Assert.Empty(template.Metadata.Tags);
    }

    [Fact]
    public void ProcessTemplateFile_ShouldTrimTagsWhitespace()
    {
        // Arrange
        var templatePath = CreateTempTemplate(@"
// META: output=generated/{{EntityName}}.cs
// META: tags=  entity  ,  model  ,  domain  

namespace Test;

public class {{EntityName}} {}
");

        // Act
        var template = _processor.ProcessTemplateFile(templatePath);

        // Assert
        Assert.Equal(3, template.Metadata.Tags.Count);
        Assert.Contains("entity", template.Metadata.Tags);
        Assert.Contains("model", template.Metadata.Tags);
        Assert.Contains("domain", template.Metadata.Tags);
    }

    private string CreateTempTemplate(string content)
    {
        var path = Path.Combine(_tempDir, $"template-{Guid.NewGuid()}.template.cs");
        File.WriteAllText(path, content);
        return path;
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempDir))
        {
            Directory.Delete(_tempDir, true);
        }
    }
}
