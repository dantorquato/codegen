using CodeGen.Services;

if (args.Length == 0 || args.Contains("--help") || args.Contains("-h"))
{
    Console.WriteLine(@"
📝 Code Generator - Usage:

  codegen --entity <name> [options]
  codegen -e <name> [options]

Required:
  --entity, -e <name>       Name of the entity to generate (e.g., Product, User)

Options:
  --templates, -t <path>    Path to templates folder (default: templates)
  --tags, -g <tags>         Comma-separated tags to filter templates (e.g., entity,service)
  --help, -h                Show this help message

Examples:
  codegen --entity Product
  codegen -e User
  codegen -e Order -t my-templates
  codegen --entity Customer --tags entity
  codegen -e Invoice -t templates -g entity,service,controller
  
  # Legacy format (still supported):
  codegen Product
  codegen User templates
  codegen Order templates entity,service
  
Tags:
  If no tags are specified, all templates will be generated.
  If tags are specified, only templates with at least one matching tag will be generated.
  Templates can have multiple tags in their metadata: // META: TAGS = entity, service
  
This will generate files based on templates in the specified folder.
Files will be created according to the metadata specified in each template.
If a file already exists, it will be skipped.
");
    return 0;
}

// Parse arguments
string? entityName = null;
string templatesFolder = "templates";
List<string>? tags = null;
bool hasNamedArgs = args.Any(a => a.StartsWith("--") || (a.StartsWith("-") && a.Length == 2 && !char.IsDigit(a[1])));

// Check for named arguments
for (int i = 0; i < args.Length; i++)
{
    var arg = args[i];
    
    if ((arg == "--entity" || arg == "-e") && i + 1 < args.Length)
    {
        entityName = args[++i];
    }
    else if ((arg == "--templates" || arg == "-t") && i + 1 < args.Length)
    {
        templatesFolder = args[++i];
    }
    else if ((arg == "--tags" || arg == "-g") && i + 1 < args.Length)
    {
        var tagsArg = args[++i];
        tags = tagsArg.Split(',')
            .Select(t => t.Trim())
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .ToList();
    }
    else if (!hasNamedArgs && !arg.StartsWith("-") && entityName == null)
    {
        // Legacy format: first positional argument is entity name
        entityName = arg;
    }
    else if (!hasNamedArgs && !arg.StartsWith("-") && entityName != null && templatesFolder == "templates")
    {
        // Legacy format: second positional argument is templates folder
        templatesFolder = arg;
    }
    else if (!hasNamedArgs && !arg.StartsWith("-") && entityName != null && tags == null)
    {
        // Legacy format: third positional argument is tags
        tags = arg.Split(',')
            .Select(t => t.Trim())
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .ToList();
    }
}

if (string.IsNullOrWhiteSpace(entityName))
{
    Console.WriteLine("❌ Error: Entity name is required.\n");
    Console.WriteLine("Use --help or -h for usage information.\n");
    return 1;
}

var baseDir = Directory.GetCurrentDirectory();
var templatesDir = Path.Combine(baseDir, templatesFolder);

try
{
    var generator = new CodeGenerator();
    generator.Generate(templatesDir, entityName, baseDir, tags);
    return 0;
}
catch (Exception ex)
{
    Console.WriteLine($"\n❌ Error: {ex.Message}\n");
    return 1;
}

