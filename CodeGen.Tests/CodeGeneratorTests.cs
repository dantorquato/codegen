using CodeGen.Services;

namespace CodeGen.Tests;

public class CodeGeneratorTests : IDisposable
{
    private readonly CodeGenerator _generator = new();
    private readonly string _tempDir = Path.Combine(Path.GetTempPath(), $"codegen-integration-{Guid.NewGuid()}");
    private readonly string _templatesDir;

    public CodeGeneratorTests()
    {
        _templatesDir = Path.Combine(_tempDir, "templates");
        Directory.CreateDirectory(_templatesDir);
    }

    [Fact]
    public void Generate_ShouldCreateFilesFromTemplates()
    {
        // Arrange
        CreateTemplate("model.template.cs", @"
// META: output=generated/{{EntityName}}.cs

public class {{EntityName}} 
{
    public string Name => ""{{entityName}}"";
}
");

        // Act
        _generator.Generate(_templatesDir, "Product", _tempDir);

        // Assert
        var outputPath = Path.Combine(_tempDir, "generated", "Product.cs");
        Assert.True(File.Exists(outputPath));
        
        var content = File.ReadAllText(outputPath);
        Assert.Contains("public class Product", content);
        Assert.Contains("\"product\"", content);
        Assert.DoesNotContain("{{EntityName}}", content);
    }

    [Fact]
    public void Generate_ShouldNotOverwriteExistingFiles()
    {
        // Arrange
        CreateTemplate("model.template.cs", @"
// META: output=generated/{{EntityName}}.cs

public class {{EntityName}} {}
");

        var outputPath = Path.Combine(_tempDir, "generated", "User.cs");
        Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
        File.WriteAllText(outputPath, "// Existing content");

        // Act
        _generator.Generate(_templatesDir, "User", _tempDir);

        // Assert
        var content = File.ReadAllText(outputPath);
        Assert.Equal("// Existing content", content);
    }

    [Fact]
    public void Generate_ShouldCreateMultipleFiles()
    {
        // Arrange
        CreateTemplate("model.template.cs", @"
// META: output=generated/Models/{{EntityName}}.cs

public class {{EntityName}} {}
");

        CreateTemplate("service.template.cs", @"
// META: output=generated/Services/{{EntityName}}Service.cs

public class {{EntityName}}Service {}
");

        // Act
        _generator.Generate(_templatesDir, "Order", _tempDir);

        // Assert
        var modelPath = Path.Combine(_tempDir, "generated", "Models", "Order.cs");
        var servicePath = Path.Combine(_tempDir, "generated", "Services", "OrderService.cs");
        
        Assert.True(File.Exists(modelPath));
        Assert.True(File.Exists(servicePath));
    }

    [Fact]
    public void Generate_ShouldThrowWhenTemplatesDirNotFound()
    {
        // Arrange
        var nonExistentDir = Path.Combine(_tempDir, "nonexistent");

        // Act & Assert
        Assert.Throws<DirectoryNotFoundException>(() => 
            _generator.Generate(nonExistentDir, "Test", _tempDir));
    }

    [Fact]
    public void Generate_ShouldThrowWhenNoTemplatesFound()
    {
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => 
            _generator.Generate(_templatesDir, "Test", _tempDir));
    }

    [Fact]
    public void Generate_ShouldCreateNestedDirectories()
    {
        // Arrange
        CreateTemplate("test.template.cs", @"
// META: output=generated/Deep/Nested/Path/{{EntityName}}.cs

public class {{EntityName}} {}
");

        // Act
        _generator.Generate(_templatesDir, "Test", _tempDir);

        // Assert
        var outputPath = Path.Combine(_tempDir, "generated", "Deep", "Nested", "Path", "Test.cs");
        Assert.True(File.Exists(outputPath));
    }

    private void CreateTemplate(string filename, string content)
    {
        var path = Path.Combine(_templatesDir, filename);
        File.WriteAllText(path, content);
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempDir))
        {
            Directory.Delete(_tempDir, true);
        }
    }
}
