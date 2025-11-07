using CodeGen.Models;

namespace CodeGen.Services;

/// <summary>
/// Main code generation service
/// </summary>
public class CodeGenerator
{
    private readonly TemplateProcessor _templateProcessor;
    private readonly VariableReplacer _variableReplacer;

    public CodeGenerator()
    {
        _templateProcessor = new TemplateProcessor();
        _variableReplacer = new VariableReplacer();
    }

    /// <summary>
    /// Generates code from all templates in the specified directory
    /// </summary>
    public void Generate(string templatesDir, string entityName, string baseDir, List<string>? tags = null)
    {
        if (!Directory.Exists(templatesDir))
        {
            throw new DirectoryNotFoundException($"Templates directory not found: {templatesDir}");
        }

        // Searches for all .template files recursively in subdirectories
        var templateFiles = Directory.GetFiles(templatesDir, "*.template.*", SearchOption.AllDirectories);

        if (templateFiles.Length == 0)
        {
            throw new InvalidOperationException($"No templates found in {templatesDir}");
        }

        Console.WriteLine($"\n🚀 Generating code for entity: {entityName}");
        
        if (tags != null && tags.Any())
        {
            Console.WriteLine($"🏷️  Filtering by tags: {string.Join(", ", tags)}");
        }
        
        Console.WriteLine();

        var variables = _variableReplacer.CreateVariables(entityName);
        int processedCount = 0;
        int skippedCount = 0;

        foreach (var templateFile in templateFiles)
        {
            var result = ProcessTemplate(templateFile, variables, baseDir, tags);
            if (result) processedCount++;
            else skippedCount++;
        }

        Console.WriteLine($"\n✨ Generation completed! {processedCount} file(s) generated");
        if (skippedCount > 0)
        {
            Console.WriteLine($"ℹ️  {skippedCount} template(s) skipped");
        }
        Console.WriteLine();
    }

    /// <summary>
    /// Processes a single template and generates the output file
    /// </summary>
    /// <returns>True if the template was processed and file generated, false if skipped</returns>
    private bool ProcessTemplate(string templateFile, Dictionary<string, string> variables, string baseDir, List<string>? tags)
    {
        try
        {
            // Reads and processes the template
            var template = _templateProcessor.ProcessTemplateFile(templateFile);

            // Validates metadata
            if (!template.Metadata.IsValid)
            {
                Console.WriteLine($"⚠️  Template {Path.GetFileName(templateFile)} does not have 'output' metadata. Skipping.");
                return false;
            }

            // Filter by tags if specified
            if (tags != null && tags.Any())
            {
                // If template has no tags, skip it when filtering
                if (!template.Metadata.Tags.Any())
                {
                    return false;
                }

                // If template doesn't have any of the requested tags, skip it
                if (!template.Metadata.HasAnyTag(tags))
                {
                    return false;
                }
            }

            // Replaces variables in content
            var content = _variableReplacer.Replace(template.Content, variables);

            // Replaces variables in output path
            var outputPath = Path.Combine(baseDir, _variableReplacer.Replace(template.Metadata.Output!, variables));

            // Checks if file already exists
            if (File.Exists(outputPath))
            {
                Console.WriteLine($"⏭️  File {outputPath} already exists. Skipping.");
                return false;
            }

            // Creates necessary directories
            var outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Writes the file
            File.WriteAllText(outputPath, content);
            
            var tagsInfo = template.Metadata.Tags.Any() 
                ? $" [{string.Join(", ", template.Metadata.Tags)}]" 
                : "";
            Console.WriteLine($"✅ File generated: {outputPath}{tagsInfo}");
            
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error processing template {Path.GetFileName(templateFile)}: {ex.Message}");
            return false;
        }
    }
}
