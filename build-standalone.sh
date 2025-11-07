#!/bin/bash

# Script to generate standalone executable (doesn't require .NET installed)

echo "🚀 Generating standalone executable..."
echo ""

# Detects platform
if [[ "$OSTYPE" == "darwin"* ]]; then
    if [[ $(uname -m) == "arm64" ]]; then
        RUNTIME="osx-arm64"
        PLATFORM="macOS Apple Silicon"
    else
        RUNTIME="osx-x64"
        PLATFORM="macOS Intel"
    fi
elif [[ "$OSTYPE" == "linux-gnu"* ]]; then
    if [[ $(uname -m) == "aarch64" ]]; then
        RUNTIME="linux-arm64"
        PLATFORM="Linux ARM64"
    else
        RUNTIME="linux-x64"
        PLATFORM="Linux x64"
    fi
else
    echo "❌ Unsupported platform: $OSTYPE"
    exit 1
fi

echo "📦 Platform detected: $PLATFORM ($RUNTIME)"
echo "⚙️  Compiling with included runtime (self-contained)..."
echo ""

dotnet publish CodeGen/CodeGen.csproj \
    -c Release \
    -r $RUNTIME \
    --self-contained true \
    -p:PublishSingleFile=true \
    -p:PublishTrimmed=true \
    -p:IncludeNativeLibrariesForSelfExtract=true \
    -o "./dist"

if [ $? -eq 0 ]; then
    chmod +x "./dist/CodeGen"
    SIZE=$(du -h "./dist/CodeGen" | cut -f1)
    
    echo ""
    echo "✅ Build completed successfully!"
    echo ""
    echo "📂 Executable: ./dist/CodeGen"
    echo "📏 Size: $SIZE"
    echo ""
    echo "✨ This executable does NOT require .NET installed!"
    echo ""
    echo "To test:"
    echo "   ./dist/CodeGen Product"
    echo ""
    echo "To install globally:"
    echo "   sudo cp ./dist/CodeGen /usr/local/bin/codegen"
    echo ""
else
    echo ""
    echo "❌ Build failed"
    exit 1
fi
