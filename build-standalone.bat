@echo off
REM Script to generate standalone executable (doesn't require .NET installed)

echo.
echo 🚀 Generating standalone executable...
echo.

REM Detect architecture
if "%PROCESSOR_ARCHITECTURE%"=="ARM64" (
    set RUNTIME=win-arm64
    set PLATFORM=Windows ARM64
) else (
    set RUNTIME=win-x64
    set PLATFORM=Windows x64
)

echo 📦 Platform detected: %PLATFORM% (%RUNTIME%)
echo ⚙️  Compiling with included runtime (self-contained)...
echo.

dotnet publish CodeGen\CodeGen.csproj ^
    -c Release ^
    -r %RUNTIME% ^
    --self-contained true ^
    -p:PublishSingleFile=true ^
    -p:PublishTrimmed=true ^
    -p:IncludeNativeLibrariesForSelfExtract=true ^
    -o ".\dist"

if %errorlevel%==0 (
    echo.
    echo ✅ Build completed successfully!
    echo.
    echo 📂 Executable: .\dist\CodeGen.exe
    echo.
    echo ✨ This executable does NOT require .NET installed!
    echo.
    echo To test:
    echo    .\dist\CodeGen.exe Product
    echo.
    echo To use globally, add .\dist to PATH
    echo.
) else (
    echo.
    echo ❌ Build failed
    exit /b 1
)

pause
