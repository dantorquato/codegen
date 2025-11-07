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
    public void Generate(string templatesDir, string entityName, string baseDir)
    {
        if (!Directory.Exists(templatesDir))
        {
            throw new DirectoryNotFoundException($"Templates directory not found: {templatesDir}");
        }

        // Searches for all .template files
        var templateFiles = Directory.GetFiles(templatesDir, "*.template.*", SearchOption.TopDirectoryOnly);

        if (templateFiles.Length == 0)
        {
            throw new InvalidOperationException($"No templates found in {templatesDir}");
        }

        Console.WriteLine($"\n🚀 Generating code for entity: {entityName}\n");

        var variables = _variableReplacer.CreateVariables(entityName);

        foreach (var templateFile in templateFiles)
        {
            ProcessTemplate(templateFile, variables, baseDir);
        }

        Console.WriteLine($"\n✨ Generation completed!\n");
    }

    /// <summary>
    /// Processes a single template and generates the output file
    /// </summary>
    private void ProcessTemplate(string templateFile, Dictionary<string, string> variables, string baseDir)
    {
        try
        {
            // Reads and processes the template
            var template = _templateProcessor.ProcessTemplateFile(templateFile);

            // Validates metadata
            if (!template.Metadata.IsValid)
            {
                Console.WriteLine($"⚠️  Template {Path.GetFileName(templateFile)} does not have 'output' metadata. Skipping.");
                return;
            }

            // Replaces variables in content
            var content = _variableReplacer.Replace(template.Content, variables);

            // Replaces variables in output path
            var outputPath = Path.Combine(baseDir, _variableReplacer.Replace(template.Metadata.Output!, variables));

            // Checks if file already exists
            if (File.Exists(outputPath))
            {
                Console.WriteLine($"⏭️  File {outputPath} already exists. Skipping.");
                return;
            }

            // Creates necessary directories
            var outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Writes the file
            File.WriteAllText(outputPath, content);
            Console.WriteLine($"✅ File generated: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error processing template {Path.GetFileName(templateFile)}: {ex.Message}");
        }
    }
}
