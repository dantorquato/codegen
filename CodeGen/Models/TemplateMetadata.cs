namespace CodeGen.Models;

/// <summary>
/// Represents metadata extracted from a template
/// </summary>
public class TemplateMetadata
{
    /// <summary>
    /// Output path where the file will be created (may contain variables)
    /// </summary>
    public string? Output { get; set; }

    /// <summary>
    /// Optional template description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Optional tags for filtering templates (comma-separated)
    /// </summary>
    public List<string> Tags { get; set; } = new List<string>();

    /// <summary>
    /// Checks if essential metadata is present
    /// </summary>
    public bool IsValid => !string.IsNullOrWhiteSpace(Output);

    /// <summary>
    /// Checks if template has any of the specified tags
    /// </summary>
    public bool HasAnyTag(IEnumerable<string> requestedTags)
    {
        if (!Tags.Any()) return false;
        return Tags.Any(t => requestedTags.Contains(t, StringComparer.OrdinalIgnoreCase));
    }
}
