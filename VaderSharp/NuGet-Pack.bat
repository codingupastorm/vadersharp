@echo off

set version=%1

dotnet restore .\VaderSharp
dotnet pack .\VaderSharp\VaderSharp.csproj --output ..\nupkgs --configuration Release /p:PackageVersion=%version% --include-symbols --include-source