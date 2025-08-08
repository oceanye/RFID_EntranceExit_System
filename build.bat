@echo off
REM UHFReader288Demo Build Script
REM Usage: build.bat [Configuration] [Platform]

setlocal enabledelayedexpansion

REM Default values
set CONFIGURATION=Release
set PLATFORM=x86

REM Parse arguments
if not "%~1"=="" set CONFIGURATION=%~1
if not "%~2"=="" set PLATFORM=%~2

REM Find MSBuild
set MSBUILD_PATH=
set MSBUILD_PATHS[0]=C:\Program Files (x86)\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\MSBuild.exe
set MSBUILD_PATHS[1]=C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe
set MSBUILD_PATHS[2]=C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe

for /L %%i in (0,1,2) do (
    if exist "!MSBUILD_PATHS[%%i]!" (
        set MSBUILD_PATH=!MSBUILD_PATHS[%%i]!
        goto :found_msbuild
    )
)

if "%MSBUILD_PATH%"=="" (
    echo Error: MSBuild not found. Please install Visual Studio or Build Tools.
    exit /b 1
)

:found_msbuild
echo Using MSBuild: %MSBUILD_PATH%
echo Building UHFReader288Demo - Configuration: %CONFIGURATION%, Platform: %PLATFORM%

REM Build the project
"%MSBUILD_PATH%" UHFReader288Demo.sln /p:Configuration=%CONFIGURATION% /p:Platform=%PLATFORM%

if %ERRORLEVEL% NEQ 0 (
    echo Build failed!
    exit /b %ERRORLEVEL%
)

echo Build completed successfully!
set OUTPUT_PATH=bin\%PLATFORM%\%CONFIGURATION%\UHFReader288Demo.exe
if exist "%OUTPUT_PATH%" (
    for %%A in ("%OUTPUT_PATH%") do (
        echo Output: %OUTPUT_PATH% (%%~zA bytes)
    )
)

pause
