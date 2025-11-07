namespace CodeGen.Models;

/// <summary>
/// Represents a template with its content and metadata
/// </summary>
public class Template
{
    /// <summary>
    /// Template file path
    /// </summary>
    public string FilePath { get; set; } = string.Empty;

    /// <summary>
    /// Template content (without metadata lines)
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Metadata extracted from the template
    /// </summary>
    public TemplateMetadata Metadata { get; set; } = new();
}
