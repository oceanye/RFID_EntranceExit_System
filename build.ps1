# UHFReader288Demo Build Script
# This script builds the UHFReader288Demo project for different platforms and configurations

param(
    [string]$Configuration = "Release",
    [string]$Platform = "x86",
    [switch]$Clean,
    [switch]$Help
)

if ($Help) {
    Write-Host "UHFReader288Demo Build Script"
    Write-Host "Usage: .\build.ps1 [-Configuration <Debug|Release>] [-Platform <x86|x64|AnyCPU>] [-Clean] [-Help]"
    Write-Host ""
    Write-Host "Examples:"
    Write-Host "  .\build.ps1                          # Build Release x86"
    Write-Host "  .\build.ps1 -Configuration Debug     # Build Debug x86"
    Write-Host "  .\build.ps1 -Platform x64            # Build Release x64"
    Write-Host "  .\build.ps1 -Clean                   # Clean before build"
    Write-Host "  .\build.ps1 -Help                    # Show this help"
    exit 0
}

# Find MSBuild
$msbuildPaths = @(
    "C:\Program Files (x86)\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\MSBuild.exe",
    "C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe",
    "C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe"
)

$msbuild = $null
foreach ($path in $msbuildPaths) {
    if (Test-Path $path) {
        $msbuild = $path
        break
    }
}

if (-not $msbuild) {
    Write-Error "MSBuild not found. Please install Visual Studio or Build Tools."
    exit 1
}

Write-Host "Using MSBuild: $msbuild"
Write-Host "Building UHFReader288Demo - Configuration: $Configuration, Platform: $Platform"

# Clean if requested
if ($Clean) {
    Write-Host "Cleaning project..."
    & $msbuild UHFReader288Demo.sln /t:Clean /p:Configuration=$Configuration /p:Platform=$Platform
}

# Build the project
& $msbuild UHFReader288Demo.sln /p:Configuration=$Configuration /p:Platform=$Platform

if ($LASTEXITCODE -eq 0) {
    Write-Host "Build completed successfully!" -ForegroundColor Green
    
    # Show output location
    $outputPath = "bin\$Platform\$Configuration\UHFReader288Demo.exe"
    if (Test-Path $outputPath) {
        $fileInfo = Get-Item $outputPath
        Write-Host "Output: $outputPath ($([math]::Round($fileInfo.Length/1KB, 2)) KB)"
    }
} else {
    Write-Host "Build failed!" -ForegroundColor Red
    exit $LASTEXITCODE
}
