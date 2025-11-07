namespace CodeGen.Models;

/// <summary>
/// Represents metadata extracted from a template
/// </summary>
public class TemplateMetadata
{
    /// <summary>
    /// Name of the file to be generated (may contain variables)
    /// </summary>
    public string? FileName { get; set; }

    /// <summary>
    /// Output path where the file will be created (may contain variables)
    /// </summary>
    public string? Output { get; set; }

    /// <summary>
    /// Optional template description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Checks if essential metadata is present
    /// </summary>
    public bool IsValid => !string.IsNullOrWhiteSpace(Output);
}
