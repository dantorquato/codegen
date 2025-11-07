using CodeGen.Services;

if (args.Length == 0)
{
    Console.WriteLine(@"
📝 Code Generator - Usage:

  codegen <EntityName> [TemplatesFolder]

Arguments:
  EntityName        Name of the entity to generate (e.g., Product, User)
  TemplatesFolder   Optional: Path to templates folder (default: templates)

Examples:
  codegen Product
  codegen User
  codegen Order
  codegen Customer my-templates
  codegen Invoice ./custom-templates
  
This will generate files based on templates in the specified folder.
Files will be created according to the metadata specified in each template.
If a file already exists, it will be skipped.
");
    return 0;
}

var entityName = args[0];
var templatesFolder = args.Length > 1 ? args[1] : "templates";
var baseDir = Directory.GetCurrentDirectory();
var templatesDir = Path.Combine(baseDir, templatesFolder);

try
{
    var generator = new CodeGenerator();
    generator.Generate(templatesDir, entityName, baseDir);
    return 0;
}
catch (Exception ex)
{
    Console.WriteLine($"\n❌ Error: {ex.Message}\n");
    return 1;
}

