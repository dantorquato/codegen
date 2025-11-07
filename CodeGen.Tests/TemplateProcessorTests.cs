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
// META: filename={{EntityName}}.cs
// META: output=generated/Models/{{EntityName}}.cs
// META: description=Model class

namespace Test;

public class {{EntityName}} {}
");

        // Act
        var template = _processor.ProcessTemplateFile(templatePath);

        // Assert
        Assert.Equal("{{EntityName}}.cs", template.Metadata.FileName);
        Assert.Equal("generated/Models/{{EntityName}}.cs", template.Metadata.Output);
        Assert.Equal("Model class", template.Metadata.Description);
    }

    [Fact]
    public void ProcessTemplateFile_ShouldRemoveMetadataFromContent()
    {
        // Arrange
        var templatePath = CreateTempTemplate(@"
// META: filename=test.cs
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
        Assert.Null(template.Metadata.FileName);
        Assert.Null(template.Metadata.Output);
        Assert.Null(template.Metadata.Description);
        Assert.False(template.Metadata.IsValid);
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
