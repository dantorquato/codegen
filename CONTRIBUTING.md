# Contributing to CodeGen

First off, thank you for considering contributing to CodeGen! It's people like you that make CodeGen such a great tool.

## Code of Conduct

This project and everyone participating in it is governed by our Code of Conduct. By participating, you are expected to uphold this code.

## How Can I Contribute?

### Reporting Bugs

Before creating bug reports, please check the existing issues as you might find out that you don't need to create one. When you are creating a bug report, please include as many details as possible:

* **Use a clear and descriptive title**
* **Describe the exact steps which reproduce the problem**
* **Provide specific examples to demonstrate the steps**
* **Describe the behavior you observed after following the steps**
* **Explain which behavior you expected to see instead and why**
* **Include screenshots if possible**
* **Include your environment details** (OS, .NET version, etc.)

### Suggesting Enhancements

Enhancement suggestions are tracked as GitHub issues. When creating an enhancement suggestion, please include:

* **Use a clear and descriptive title**
* **Provide a step-by-step description of the suggested enhancement**
* **Provide specific examples to demonstrate the steps**
* **Describe the current behavior and explain which behavior you expected to see instead**
* **Explain why this enhancement would be useful**

### Pull Requests

* Fill in the required template
* Do not include issue numbers in the PR title
* Include screenshots and animated GIFs in your pull request whenever possible
* Follow the C# coding style (see below)
* Include thoughtfully-worded, well-structured tests
* Document new code
* End all files with a newline

## Development Process

### Setting Up Your Development Environment

1. **Fork the Repository**
   - Click "Fork" at the top right of the repository page

2. **Clone Your Fork**
   ```bash
   git clone https://github.com/dantorquato/codegen.git
   cd codegen
   ```
3. Add upstream remote:
   ```bash
   git remote add upstream https://github.com/ORIGINAL-OWNER/code-gen.git
   ```
4. Create a branch:
   ```bash
   git checkout -b feature/my-new-feature
   ```

### Building and Testing

```bash
# Build the project
dotnet build

# Run tests
dotnet test

# Run in development mode
dotnet run --project CodeGen/CodeGen.csproj <EntityName>
```

### Coding Standards

* Follow the existing code style
* Use meaningful variable and method names
* Write XML documentation comments for public APIs
* Keep methods focused and small
* Write unit tests for new functionality
* Update documentation as needed

### C# Style Guide

* Use PascalCase for public members
* Use camelCase for private fields (prefix with `_`)
* Use clear, descriptive names
* Place `{` on a new line
* Use explicit types (avoid `var` unless type is obvious)
* One class per file
* Group using statements and sort alphabetically

Example:

```csharp
namespace CodeGen.Services;

/// <summary>
/// Service description
/// </summary>
public class MyService
{
    private readonly IDependency _dependency;

    public MyService(IDependency dependency)
    {
        _dependency = dependency ?? throw new ArgumentNullException(nameof(dependency));
    }

    /// <summary>
    /// Method description
    /// </summary>
    public void DoSomething(string parameter)
    {
        // Implementation
    }
}
```

### Commit Messages

* Use the present tense ("Add feature" not "Added feature")
* Use the imperative mood ("Move cursor to..." not "Moves cursor to...")
* Limit the first line to 72 characters or less
* Reference issues and pull requests liberally after the first line
* Consider starting the commit message with an applicable emoji:
    * 🎨 `:art:` when improving the format/structure of the code
    * 🐛 `:bug:` when fixing a bug
    * ✨ `:sparkles:` when adding a new feature
    * 📝 `:memo:` when writing docs
    * 🚀 `:rocket:` when improving performance
    * ✅ `:white_check_mark:` when adding tests
    * 🔧 `:wrench:` when changing configuration files

Example:
```
✨ Add support for YAML template metadata

- Implement YAML parser for metadata
- Add tests for YAML parsing
- Update documentation

Fixes #123
```

### Testing

* Write unit tests for all new code
* Ensure all tests pass before submitting PR
* Aim for high code coverage
* Test edge cases and error conditions
* Use descriptive test names

### Documentation

* Update README.md if needed
* Add XML comments to public APIs
* Update CHANGELOG.md
* Create examples for new features

## Project Structure

```
CodeGen/
├── Models/           # Data models
├── Services/         # Business logic
└── Program.cs        # CLI entry point

CodeGen.Tests/        # All tests
templates/            # Example templates
```

## Adding New Features

### Adding a New Template

1. Create a new `.template.*` file in `templates/`
2. Add appropriate metadata
3. Test the template with various entity names
4. Update README.md with template description

### Adding a New Service

1. Create the service class in `CodeGen/Services/`
2. Add XML documentation comments
3. Write unit tests in `CodeGen.Tests/`
4. Update dependency injection if needed

## Release Process

1. Update version in `CodeGen.csproj`
2. Update CHANGELOG.md
3. Create a Git tag
4. Push tag to trigger release workflow
5. Verify GitHub Release is created

## Questions?

Don't hesitate to ask questions by creating an issue!

## License

By contributing, you agree that your contributions will be licensed under the MIT License.

---

Thank you for contributing! 🎉
