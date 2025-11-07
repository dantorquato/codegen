# 🚀 CodeGen

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![Tests](https://img.shields.io/badge/tests-15%20passing-success)](CodeGen.Tests/)

Generate code automatically from templates. Simple, fast, and works on any project!

## ✨ Why Use CodeGen?

- ⚡ **Super Fast** - Small (~2MB) and instant startup
- 🔒 **Safe** - Never overwrites your existing files
- 🌍 **Works Everywhere** - Windows, Linux, macOS (no installation needed)
- 📝 **Any Language** - Works with C#, TypeScript, Python, Go, anything!
- 🎨 **Smart Variables** - Auto-converts names (UserProfile → userProfile → user-profile)

## 📦 Installation

### Download for Your Platform

Go to [**Releases**](https://github.com/dantorquato/codegen/releases) and download the latest version:

- **Windows:** `codegen-win-x64.zip` or `codegen-win-arm64.zip`
- **Linux:** `codegen-linux-x64.tar.gz` or `codegen-linux-arm64.tar.gz`  
- **macOS:** `codegen-osx-x64.tar.gz` or `codegen-osx-arm64.tar.gz`

> 💡 Can't find downloads? Check [all releases here](https://github.com/dantorquato/codegen/releases)

### Choose: Local or Global?

#### Option 1: Install Globally (Recommended)

Use `codegen` from anywhere on your system.

**macOS/Linux:**
```bash
tar -xzf codegen-*.tar.gz
sudo mv CodeGen /usr/local/bin/codegen

# Now use from any directory
cd ~/my-project
codegen User
```

**Windows:**
```cmd
# Extract zip, then move to a permanent location like:
move CodeGen.exe C:\bin\codegen.exe

# Add C:\bin to your PATH environment variable
# Then use from any directory:
cd C:\my-project
codegen User
```

#### Option 2: Use Locally in Your Project

Keep CodeGen in your project directory.

**macOS/Linux:**
```bash
tar -xzf codegen-*.tar.gz
mv CodeGen my-project/codegen

# Use with relative path
cd my-project
./codegen User
```

**Windows:**
```cmd
# Extract and move to project
move CodeGen.exe my-project\codegen.exe

# Use with relative path
cd my-project
codegen.exe User
```

## 🚀 Quick Start

### 1. Create a Template

In your project, create `templates/model.template.txt`:

```csharp
// META: output=Models/{{EntityName}}.cs

public class {{EntityName}}
{
    public int Id { get; set; }
    public string Name { get; set; }
}
```

### 2. Generate Code

```bash
# If installed globally:
codegen User

# If using locally:
./codegen User        # macOS/Linux
codegen.exe User      # Windows
```

### 3. Done! ✨

```
✅ File generated: Models/User.cs
```

That's it! CodeGen created `Models/User.cs` with your template.

## � Creating Templates

Templates are text files with special comments that tell CodeGen where to save the generated file:

```csharp
// META: output=path/to/{{EntityName}}.cs

// Your code here with {{EntityName}} variable
public class {{EntityName}} { }
```

### Available Variables

When you run `codegen UserProfile`, these variables are replaced:

| Variable | Result | Example |
|----------|--------|---------|
| `{{EntityName}}` | UserProfile | Class name |
| `{{entityName}}` | userProfile | Variable name |
| `{{ENTITY_NAME}}` | USERPROFILE | Constant |
| `{{entity-name}}` | user-profile | File/URL slug |

## 💡 Examples

### Generate Multiple Files

```bash
# Global installation
codegen User
codegen Product
codegen Order

# Local installation
./codegen User        # macOS/Linux
codegen.exe User      # Windows
```

### Use Custom Template Folder

```bash
codegen User my-templates
```

### Generate Multiple Entities at Once

```bash
# macOS/Linux
for entity in User Product Order; do
  codegen $entity
done

# Windows (PowerShell)
foreach ($entity in "User","Product","Order") {
  codegen $entity
}
```

### Different Template Sets

```
project/
├── templates-api/     # REST API templates
├── templates-domain/  # Domain models
└── templates-tests/   # Unit tests
```

```bash
codegen User templates-api
codegen Product templates-domain
codegen Order templates-tests
```

## ❓ FAQ

### Global vs Local - Which should I use?

**Global (Recommended for personal use):**
- ✅ Use `codegen` from anywhere
- ✅ Simpler commands
- ❌ Team members need to install it

**Local (Recommended for teams):**
- ✅ Commit to version control
- ✅ Everyone has the same version
- ✅ Works in CI/CD pipelines
- ❌ Need to use `./codegen` or full path

### Does it overwrite my files?
No! CodeGen never overwrites existing files. It's safe to run multiple times.

### What languages does it support?
Any! CodeGen works with templates, so you can generate C#, TypeScript, Python, Go, or any other language.

### Do I need .NET installed?
No! The standalone executable includes everything needed.

### Can I use custom template folders?
Yes! Just run `codegen EntityName my-folder`

## 🛠️ For Developers

### Requirements
- .NET 9.0 SDK (for building from source)

### Build
```bash
./build-standalone.sh    # macOS/Linux
build-standalone.bat     # Windows
```

### Test
```bash
dotnet test
```

## 💬 Support

- 📫 [Report bugs or request features](https://github.com/dantorquato/codegen/issues)
- � [Download latest version](https://github.com/dantorquato/codegen/releases)
- �💡 Check the [templates folder](templates/) for examples
- ⭐ Star this repo if you find it useful!

## 📝 License

MIT License - feel free to use in your projects!

---

Made with ❤️ using .NET 9.0
