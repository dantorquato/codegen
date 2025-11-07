using CodeGen.Models;

namespace CodeGen.Services;

/// <summary>
/// Service responsible for processing templates
/// </summary>
public class TemplateProcessor
{
    private const string MetadataPrefix = "// META:";

    /// <summary>
    /// Reads and processes a template file
    /// </summary>
    public Template ProcessTemplateFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Template not found: {filePath}");
        }

        var content = File.ReadAllText(filePath);
        var metadata = ExtractMetadata(content);
        var cleanContent = RemoveMetadata(content);

        return new Template
        {
            FilePath = filePath,
            Content = cleanContent,
            Metadata = metadata
        };
    }

    /// <summary>
    /// Extracts metadata from lines starting with // META:
    /// </summary>
    private TemplateMetadata ExtractMetadata(string content)
    {
        var metadata = new TemplateMetadata();
        var lines = content.Split('\n');

        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();
            if (!trimmedLine.StartsWith(MetadataPrefix)) continue;

            var metaContent = trimmedLine.Substring(MetadataPrefix.Length).Trim();
            var parts = metaContent.Split('=', 2);

            if (parts.Length != 2) continue;

            var key = parts[0].Trim().ToLower();
            var value = parts[1].Trim();

            switch (key)
            {
                case "output":
                    metadata.Output = value;
                    break;
                case "description":
                    metadata.Description = value;
                    break;
                case "tags":
                    metadata.Tags = value.Split(',')
                        .Select(t => t.Trim())
                        .Where(t => !string.IsNullOrWhiteSpace(t))
                        .ToList();
                    break;
            }
        }

        return metadata;
    }

    /// <summary>
    /// Removes metadata lines from content
    /// </summary>
    private string RemoveMetadata(string content)
    {
        var lines = content.Split('\n')
            .Where(line => !line.Trim().StartsWith(MetadataPrefix))
            .ToArray();

        return string.Join('\n', lines).Trim();
    }
}
