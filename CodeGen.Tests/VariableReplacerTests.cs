using CodeGen.Services;

namespace CodeGen.Tests;

public class VariableReplacerTests
{
    private readonly VariableReplacer _replacer = new();

    [Fact]
    public void Replace_ShouldReplaceVariables()
    {
        // Arrange
        var content = "Hello {{EntityName}}, welcome {{entityName}}!";
        var variables = new Dictionary<string, string>
        {
            ["EntityName"] = "Product",
            ["entityName"] = "product"
        };

        // Act
        var result = _replacer.Replace(content, variables);

        // Assert
        Assert.Equal("Hello Product, welcome product!", result);
    }

    [Fact]
    public void Replace_ShouldReplaceMultipleOccurrences()
    {
        // Arrange
        var content = "{{EntityName}} is {{EntityName}} is {{EntityName}}";
        var variables = new Dictionary<string, string>
        {
            ["EntityName"] = "User"
        };

        // Act
        var result = _replacer.Replace(content, variables);

        // Assert
        Assert.Equal("User is User is User", result);
    }

    [Fact]
    public void CreateVariables_ShouldCreateAllVariants()
    {
        // Act
        var variables = _replacer.CreateVariables("Product");

        // Assert
        Assert.Equal("Product", variables["EntityName"]);
        Assert.Equal("product", variables["entityName"]);
        Assert.Equal("PRODUCT", variables["ENTITY_NAME"]);
        Assert.Equal("product", variables["entity-name"]);
    }

    [Fact]
    public void CreateVariables_ShouldHandlePascalCase()
    {
        // Act
        var variables = _replacer.CreateVariables("UserProfile");

        // Assert
        Assert.Equal("UserProfile", variables["EntityName"]);
        Assert.Equal("userProfile", variables["entityName"]);
        Assert.Equal("USERPROFILE", variables["ENTITY_NAME"]);
        Assert.Equal("user-profile", variables["entity-name"]);
    }

    [Fact]
    public void CreateVariables_ShouldHandleLowerCase()
    {
        // Act
        var variables = _replacer.CreateVariables("order");

        // Assert
        Assert.Equal("Order", variables["EntityName"]);
        Assert.Equal("order", variables["entityName"]);
        Assert.Equal("ORDER", variables["ENTITY_NAME"]);
        Assert.Equal("order", variables["entity-name"]);
    }
}
