# 🚀 CodeGen

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![Tests](https://img.shields.io/badge/tests-15%20passing-success)](CodeGen.Tests/)

A powerful, cross-platform code generator built with .NET 9.0. Generate boilerplate code from templates with metadata - no .NET runtime required on target machines!

## ✨ Features

- 🎯 **Template-based code generation** with metadata support
- 🌍 **Cross-platform** standalone executables (Windows, Linux, macOS)
- 🔒 **Safe** - never overwrites existing files
- 🧪 **Well-tested** - 15 automated tests, 100% passing
- ⚡ **Fast** - single-file executable (~2MB with Native AOT)
- 🚀 **Instant startup** - no JIT compilation needed
- 📝 **Flexible** - works with any programming language
- 🎨 **Variable substitution** - PascalCase, camelCase, UPPERCASE, kebab-case

## 📦 Installation

### Option 1: Download Pre-built Binary (Recommended)

Download the latest standalone executable for your platform from the [Releases page](https://github.com/dantorquato/codegen/releases/latest):

| Platform | Architecture | Download |
|----------|-------------|----------|
| 🪟 Windows | x64 | [CodeGen-win-x64.zip](https://github.com/dantorquato/codegen/releases/latest/download/codegen-win-x64.zip) |
| 🪟 Windows | ARM64 | [CodeGen-win-arm64.zip](https://github.com/dantorquato/codegen/releases/latest/download/codegen-win-arm64.zip) |
| 🐧 Linux | x64 | [CodeGen-linux-x64.tar.gz](https://github.com/dantorquato/codegen/releases/latest/download/codegen-linux-x64.tar.gz) |
| 🐧 Linux | ARM64 | [CodeGen-linux-arm64.tar.gz](https://github.com/dantorquato/codegen/releases/latest/download/codegen-linux-arm64.tar.gz) |
| 🍎 macOS | Intel | [CodeGen-osx-x64.tar.gz](https://github.com/dantorquato/codegen/releases/latest/download/codegen-osx-x64.tar.gz) |
| 🍎 macOS | Apple Silicon | [CodeGen-osx-arm64.tar.gz](https://github.com/dantorquato/codegen/releases/latest/download/codegen-osx-arm64.tar.gz) |

**No .NET installation required!** The binaries include everything needed.

#### Extract and Use

**macOS/Linux:**
```bash
# Download and extract
tar -xzf codegen-osx-arm64.tar.gz
chmod +x CodeGen

# Run
./CodeGen Product

# (Optional) Install globally
sudo mv CodeGen /usr/local/bin/codegen
codegen Product
```

**Windows:**
```cmd
# Extract the zip file
# Then run
CodeGen.exe Product

# (Optional) Add to PATH for global use
```

### Option 2: Build from Source

If you have .NET 9.0 SDK installed:

```bash
# Clone the repository
git clone https://github.com/dantorquato/codegen.git
cd codegen

# Build standalone executable
./build-standalone.sh          # macOS/Linux
build-standalone.bat           # Windows

# Run
./dist/CodeGen Product
```

### Option 3: Use with .NET SDK

If you prefer to use the .NET SDK directly:

```bash
# Clone and build
git clone https://github.com/dantorquato/codegen.git
cd codegen
dotnet build

# Run
dotnet run --project CodeGen/CodeGen.csproj Product
```

## 🚀 Quick Start

### 1. Set Up in Your Project

```bash
# Create a templates directory in your project
cd /path/to/your-project
mkdir templates

# Or use a custom folder name
mkdir my-templates

# Download or create template files
# (See "Creating Templates" section below)
```

### 2. Generate Code

```bash
# Using default 'templates' folder
codegen Product

# Using custom templates folder
codegen Product my-templates
codegen User ./custom-templates
codegen Order ../shared-templates

# If using local executable
/path/to/CodeGen Product
/path/to/CodeGen Product my-templates

# If using .NET SDK
dotnet /path/to/CodeGen.dll Product
dotnet /path/to/CodeGen.dll Product my-templates
```

### 3. Output

CodeGen will create files based on your templates:

```
✅ File generated: generated/Models/Product.cs
✅ File generated: generated/Services/ProductService.cs
✅ File generated: generated/Controllers/ProductController.cs

✨ Generation completed!
```

The files are created according to the `output` metadata in your templates.

## 📖 Using CodeGen in Your Project

### Step-by-Step Integration

#### 1. Download CodeGen

Choose your preferred method from the [Installation](#-installation) section above.

#### 2. Add Templates to Your Project

Create a `templates` folder in your project root:

```bash
cd your-project/
mkdir templates
```

#### 3. Create Your First Template

Create a file `templates/model.template.txt`:

```csharp
// META: filename={{EntityName}}.cs
// META: output=src/Models/{{EntityName}}.cs

namespace YourProject.Models;

public class {{EntityName}}
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
```

#### 4. Run CodeGen

From your project root:

```bash
# If installed globally
codegen User

# If using local executable
/path/to/CodeGen User

# Output:
# ✅ File generated: src/Models/User.cs
```

#### 5. Use the Generated Code

```csharp
using YourProject.Models;

var user = new User 
{ 
    Id = 1, 
    Name = "John Doe" 
};
```

### Integration with Build Process

#### Option A: npm/package.json (Node.js projects)

```json
{
  "scripts": {
    "codegen": "codegen",
    "codegen:custom": "codegen $npm_config_entity $npm_config_folder",
    "generate:user": "codegen User",
    "generate:product": "codegen Product my-templates"
  }
}
```

Run: 
```bash
npm run generate:user
npm run generate:product
npm run codegen:custom --entity=Order --folder=custom-templates
```

#### Option B: Makefile (C/C++, Go, Rust projects)

```makefile
.PHONY: codegen

TEMPLATES ?= templates

codegen:
	codegen $(entity) $(TEMPLATES)

generate-user:
	codegen User

generate-product:
	codegen Product my-templates
```

Run: 
```bash
make generate-user
make generate-product
make codegen entity=Order TEMPLATES=custom-templates
```

#### Option C: .NET Project Tasks

Add to your `.csproj`:

```xml
<Target Name="CodeGen" BeforeTargets="BeforeBuild">
  <PropertyGroup>
    <TemplatesFolder Condition="'$(TemplatesFolder)' == ''">templates</TemplatesFolder>
  </PropertyGroup>
  <Exec Command="codegen $(Entity) $(TemplatesFolder)" />
</Target>
```

Run: 
```bash
dotnet build /p:Entity=User
dotnet build /p:Entity=Product /p:TemplatesFolder=my-templates
```

#### Option D: Shell Script

Create `scripts/generate.sh`:

```bash
#!/bin/bash
ENTITY=$1
TEMPLATES=${2:-templates}
codegen $ENTITY $TEMPLATES
```

Run: 
```bash
./scripts/generate.sh Product
./scripts/generate.sh Order my-templates
```

### Common Use Cases

#### Generate CRUD for Multiple Entities

```bash
# Using default templates folder
for entity in User Product Order Customer; do
  codegen $entity
done

# Using custom templates folder
for entity in User Product Order Customer; do
  codegen $entity my-templates
done
```

#### Generate Only If File Doesn't Exist

CodeGen automatically skips existing files, so you can run it safely:

```bash
codegen User  # Creates User.cs
codegen User  # Skips (file already exists)
```

#### Different Templates for Different Contexts

Structure your templates:

```
my-project/
├── templates-api/
│   ├── controller.template.txt
│   └── dto.template.txt
├── templates-domain/
│   ├── entity.template.txt
│   └── repository.template.txt
└── templates-tests/
    └── test.template.txt
```

Generate from specific folders:

```bash
codegen User templates-api
codegen Product templates-domain
codegen Order templates-tests
```

### Tips for Team Collaboration

1. **Commit templates to version control**
   ```bash
   git add templates/
   git commit -m "Add code generation templates"
   ```

2. **Document your templates**
   ```bash
   # Add README in templates/
   echo "# Templates" > templates/README.md
   echo "- model.template.txt: Generates domain models" >> templates/README.md
   ```

3. **Add generated files to .gitignore (optional)**
   ```gitignore
   # If you want to regenerate on each clone
   generated/
   ```

4. **Or commit generated files**
   ```bash
   # If generated files are part of your codebase
   git add generated/
   git commit -m "Add generated models"
   ```

### Example: Full Project Setup

```bash
# 1. Start new project
mkdir my-awesome-api
cd my-awesome-api

# 2. Download CodeGen
curl -L https://github.com/dantorquato/codegen/releases/latest/download/codegen-linux-x64.tar.gz | tar xz
sudo mv CodeGen /usr/local/bin/codegen

# 3. Create templates folder (or use custom name)
mkdir templates
# or: mkdir my-custom-templates

# 4. Add your templates
cat > templates/model.template.txt << 'EOF'
// META: output=Models/{{EntityName}}.cs
namespace MyApi.Models;
public class {{EntityName}} 
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
EOF

# 5. Generate code
codegen Product
codegen User
codegen Order

# Or with custom templates folder:
# codegen Product my-custom-templates

# 6. Check results
ls -la Models/
# Product.cs, User.cs, Order.cs

# 7. Done! Start coding
```

### Creating Templates

Templates are simple text files with special metadata comments:

```csharp
// META: filename={{EntityName}}.cs
// META: output=generated/Models/{{EntityName}}.cs
// META: description=Model with interface

namespace Generated.Models;

public interface I{{EntityName}}
{
    Guid Id { get; set; }
    string Name { get; set; }
}

public class {{EntityName}} : I{{EntityName}}
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
```

### Available Variables

When generating code for "UserProfile":

| Variable | Result | Case Style |
|----------|--------|------------|
| `{{EntityName}}` | `UserProfile` | PascalCase |
| `{{entityName}}` | `userProfile` | camelCase |
| `{{ENTITY_NAME}}` | `USERPROFILE` | UPPERCASE |
| `{{entity-name}}` | `user-profile` | kebab-case |

### Template Metadata

Only `output` is required:

- `filename` - Name of the generated file (optional)
- `output` - Path where file will be created (required)
- `description` - Template description (optional)

## 🏗️ Project Structure

```
CodeGen/                # Main application
├── Models/            # Data models
├── Services/          # Business logic
└── Program.cs         # CLI entry point

CodeGen.Tests/         # Automated tests (xUnit)
├── VariableReplacerTests.cs
├── TemplateProcessorTests.cs
└── CodeGeneratorTests.cs

templates/             # Template files
├── model.template.txt
├── service.template.txt
└── controller.template.txt

generated/             # Generated code output
```

## 🧪 Running Tests

```bash
# Run all tests
dotnet test

# Run with detailed output
dotnet test -v detailed

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"
```

**Test Results:** 15/15 passing ✅

## 🔨 Building

### Quick build (current platform)

```bash
./build-standalone.sh    # macOS/Linux
build-standalone.bat     # Windows
```

### Build for all platforms

```bash
./build-all.sh
```

Generates executables for:
- Windows (x64, ARM64)
- Linux (x64, ARM64)
- macOS (Intel, Apple Silicon)

### Development mode

```bash
# Run directly with .NET SDK
dotnet run --project CodeGen/CodeGen.csproj Product

# Build debug version
dotnet build
```

## 🌍 Platform Support

| Platform | Architecture | Executable | Size |
|----------|-------------|------------|------|
| Windows | x64 | CodeGen.exe | ~2MB |
| Windows | ARM64 | CodeGen.exe | ~2MB |
| Linux | x64 | CodeGen | ~2MB |
| Linux | ARM64 | CodeGen | ~2MB |
| macOS | Intel | CodeGen | ~2MB |
| macOS | Apple Silicon | CodeGen | ~2MB |

**Note:** Standalone executables include the .NET runtime - no installation required!  
**Optimized:** .NET 9 with Native AOT, full trimming, compression, and size optimizations.

## 💡 Usage Examples

### Generate multiple entities

```bash
./dist/CodeGen Product
./dist/CodeGen Customer
./dist/CodeGen Order
```

### Example output

```
🚀 Generating code for entity: Product

✅ File generated: generated/Models/Product.cs
✅ File generated: generated/Services/ProductService.cs
✅ File generated: generated/Controllers/ProductController.cs

✨ Generation completed!
```

### If you run again

```
🚀 Generating code for entity: Product

⏭️  File generated/Models/Product.cs already exists. Skipping.
⏭️  File generated/Services/ProductService.cs already exists. Skipping.
⏭️  File generated/Controllers/ProductController.cs already exists. Skipping.

✨ Generation completed!
```

## 🔧 Global Installation (Optional)

### macOS/Linux

```bash
sudo cp ./dist/CodeGen /usr/local/bin/codegen
```

Now use from anywhere:

```bash
cd ~/my-project
codegen Order
```

### Windows

1. Add `C:\path\to\code-gen\dist` to your PATH environment variable
2. Use from anywhere:

```cmd
cd C:\my-project
CodeGen.exe Order
```

## 🎓 What's Included

Three production-ready C# templates:

1. **Model** - Interface and class with timestamps
2. **Service** - Complete CRUD operations (Create, Read, Update, Delete)
3. **Controller** - REST API endpoints with ASP.NET Core

## 🛠️ Requirements

### To use the executable
- **None!** The standalone executable includes everything needed.

### For development
- .NET 9.0 SDK or higher
- Visual Studio Code (recommended) or Visual Studio

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the project
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🌟 Why CodeGen?

- **Saves time** - Generate repetitive boilerplate code instantly
- **Consistent** - Ensures consistent code structure across your project
- **Customizable** - Create templates for any language or framework
- **Standalone** - No dependencies or runtime installation required
- **Safe** - Never overwrites your modified code
- **Well-tested** - Comprehensive test suite ensures reliability

## 🚀 Roadmap

- [ ] YAML/JSON metadata support for complex configurations
- [ ] Automatic test generation alongside code
- [ ] Multiple template directory support
- [ ] Watch mode (regenerate on template changes)
- [ ] CI/CD integration examples
- [ ] Plugin system for custom transformations
- [ ] GUI (Graphical User Interface)
- [ ] Template marketplace/remote repository

## 📊 Stats

- **Code files:** 15 C# files
- **Lines of code:** ~1,000 LOC
- **Tests:** 15 tests, 0 failures
- **Templates included:** 3 (model, service, controller)
- **Supported platforms:** 6 (win-x64, win-arm64, linux-x64, linux-arm64, osx-x64, osx-arm64)
- **Executable size:** ~2MB (with .NET 9 Native AOT)
- **Startup time:** < 10ms (Native AOT - no JIT)
- **Optimizations:** Native AOT, full trimming, compression, globalization removal, symbol stripping

## 💬 Support

- 📫 Create an [issue](https://github.com/dantorquato/codegen/issues) for bug reports or feature requests
- 💡 Check out the [examples](templates/) for template inspiration
- 📚 Read the [tests](CodeGen.Tests/) for usage examples

---

Made with ❤️ using .NET 9.0
