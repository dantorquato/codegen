using System.Text.RegularExpressions;

namespace CodeGen.Services;

/// <summary>
/// Service responsible for replacing variables in templates
/// </summary>
public class VariableReplacer
{
    /// <summary>
    /// Replaces variables in {{VariableName}} format in the content
    /// </summary>
    public string Replace(string content, Dictionary<string, string> variables)
    {
        var result = content;

        foreach (var (key, value) in variables)
        {
            var pattern = $@"{{\{{{key}\}}}}";
            result = Regex.Replace(result, pattern, value);
        }

        return result;
    }

    /// <summary>
    /// Creates a dictionary of variables based on the entity name
    /// </summary>
    public Dictionary<string, string> CreateVariables(string entityName)
    {
        return new Dictionary<string, string>
        {
            ["EntityName"] = ToPascalCase(entityName),
            ["entityName"] = ToCamelCase(entityName),
            ["ENTITY_NAME"] = ToUpperCase(entityName),
            ["entity-name"] = ToKebabCase(entityName)
        };
    }

    private string ToPascalCase(string str)
    {
        if (string.IsNullOrEmpty(str)) return str;
        return char.ToUpper(str[0]) + str.Substring(1);
    }

    private string ToCamelCase(string str)
    {
        if (string.IsNullOrEmpty(str)) return str;
        return char.ToLower(str[0]) + str.Substring(1);
    }

    private string ToUpperCase(string str)
    {
        return str.ToUpper();
    }

    private string ToKebabCase(string str)
    {
        if (string.IsNullOrEmpty(str)) return str;
        
        var result = Regex.Replace(str, "([A-Z])", "-$1");
        return result.TrimStart('-').ToLower();
    }
}
