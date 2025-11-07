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

    [Fact]
    public void Generate_ShouldFilterByTags_WhenSingleTagProvided()
    {
        // Arrange
        CreateTemplate("model.template.cs", @"
// META: output=generated/Models/{{EntityName}}.cs
// META: tags=entity, model, domain

public class {{EntityName}} {}
");

        CreateTemplate("controller.template.cs", @"
// META: output=generated/Controllers/{{EntityName}}Controller.cs
// META: tags=entity, controller, api, service

public class {{EntityName}}Controller {}
");

        CreateTemplate("service.template.cs", @"
// META: output=generated/Services/{{EntityName}}Service.cs
// META: tags=entity, service, business

public class {{EntityName}}Service {}
");

        // Act
        var tags = new List<string> { "model" };
        _generator.Generate(_templatesDir, "Uzumaki", _tempDir, tags);

        // Assert
        var modelPath = Path.Combine(_tempDir, "generated", "Models", "Uzumaki.cs");
        var controllerPath = Path.Combine(_tempDir, "generated", "Controllers", "UzumakiController.cs");
        var servicePath = Path.Combine(_tempDir, "generated", "Services", "UzumakiService.cs");
        
        Assert.True(File.Exists(modelPath), "Model should be generated (has 'model' tag)");
        Assert.False(File.Exists(controllerPath), "Controller should NOT be generated (no 'model' tag)");
        Assert.False(File.Exists(servicePath), "Service should NOT be generated (no 'model' tag)");
    }

    [Fact]
    public void Generate_ShouldFilterByTags_WhenMultipleTagsProvided()
    {
        // Arrange
        CreateTemplate("model.template.cs", @"
// META: output=generated/Models/{{EntityName}}.cs
// META: tags=entity, model, domain

public class {{EntityName}} {}
");

        CreateTemplate("controller.template.cs", @"
// META: output=generated/Controllers/{{EntityName}}Controller.cs
// META: tags=entity, controller, api, service

public class {{EntityName}}Controller {}
");

        CreateTemplate("service.template.cs", @"
// META: output=generated/Services/{{EntityName}}Service.cs
// META: tags=entity, service, business

public class {{EntityName}}Service {}
");

        // Act - Filter by "model" OR "api"
        var tags = new List<string> { "model", "api" };
        _generator.Generate(_templatesDir, "Uzumaki", _tempDir, tags);

        // Assert
        var modelPath = Path.Combine(_tempDir, "generated", "Models", "Uzumaki.cs");
        var controllerPath = Path.Combine(_tempDir, "generated", "Controllers", "UzumakiController.cs");
        var servicePath = Path.Combine(_tempDir, "generated", "Services", "UzumakiService.cs");
        
        Assert.True(File.Exists(modelPath), "Model should be generated (has 'model' tag)");
        Assert.True(File.Exists(controllerPath), "Controller should be generated (has 'api' tag)");
        Assert.False(File.Exists(servicePath), "Service should NOT be generated (no 'model' or 'api' tag)");
    }

    [Fact]
    public void Generate_ShouldGenerateAll_WhenNoTagsProvided()
    {
        // Arrange
        CreateTemplate("model.template.cs", @"
// META: output=generated/Models/{{EntityName}}.cs
// META: tags=entity, model

public class {{EntityName}} {}
");

        CreateTemplate("service.template.cs", @"
// META: output=generated/Services/{{EntityName}}Service.cs
// META: tags=entity, service

public class {{EntityName}}Service {}
");

        // Act - No tags filter
        _generator.Generate(_templatesDir, "Product", _tempDir, null);

        // Assert
        var modelPath = Path.Combine(_tempDir, "generated", "Models", "Product.cs");
        var servicePath = Path.Combine(_tempDir, "generated", "Services", "ProductService.cs");
        
        Assert.True(File.Exists(modelPath), "Model should be generated (no filter)");
        Assert.True(File.Exists(servicePath), "Service should be generated (no filter)");
    }

    [Fact]
    public void Generate_ShouldSkipTemplatesWithoutTags_WhenFilteringByTags()
    {
        // Arrange
        CreateTemplate("model.template.cs", @"
// META: output=generated/Models/{{EntityName}}.cs
// META: tags=entity, model

public class {{EntityName}} {}
");

        CreateTemplate("untagged.template.cs", @"
// META: output=generated/{{EntityName}}.cs

public class {{EntityName}} {}
");

        // Act - Filter by "model"
        var tags = new List<string> { "model" };
        _generator.Generate(_templatesDir, "Product", _tempDir, tags);

        // Assert
        var modelPath = Path.Combine(_tempDir, "generated", "Models", "Product.cs");
        var untaggedPath = Path.Combine(_tempDir, "generated", "Product.cs");
        
        Assert.True(File.Exists(modelPath), "Model should be generated (has 'model' tag)");
        Assert.False(File.Exists(untaggedPath), "Untagged template should be skipped when filtering");
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
