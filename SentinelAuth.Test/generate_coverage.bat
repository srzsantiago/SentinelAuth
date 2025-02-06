@echo off
echo Running Coverlet...
coverlet .\bin\Debug\net8.0\SentinelAuth.Test.dll --target "dotnet" --targetargs "test .\bin\Debug\net8.0\SentinelAuth.Test.dll --no-build" --format cobertura -o coverage.xml

if %errorlevel% neq 0 (
    echo Coverlet failed! Exiting...
    exit /b %errorlevel%
)

echo Running ReportGenerator...
reportgenerator -reports:coverage.xml -targetdir:CoverageReport -reporttypes:Html

if %errorlevel% neq 0 (
    echo ReportGenerator failed! Exiting...
    exit /b %errorlevel%
)

echo Coverage report generated successfully!
pause
