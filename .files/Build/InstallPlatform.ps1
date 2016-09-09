<#
.Synopsis
    Installs InfinniPlatform to the solution output directory.
#>
param
(
    [Parameter(HelpMessage = "Name of the solution.")]
    [String] $solutionName,

    [Parameter(HelpMessage = "Path to the solution directory.")]
    [String] $solutionDir,

    [Parameter(HelpMessage = "Path to the solution output directory.")]
    [String] $solutionOutDir,

    [Parameter(HelpMessage = "Active solution configuration.")]
    [String] $solutionConfig = 'Debug',

    [Parameter(HelpMessage = "Version of the .NET.")]
    [String] $framework = 'net452'
)

# Run script in Debug mode only

if (-not ($solutionConfig -like 'Debug'))
{
    return
}

# Install NuGet

$nugetDir = Join-Path $env:ProgramData 'NuGet'
$nugetPath = Join-Path $nugetDir 'nuget.exe'

if (-not (Test-Path $nugetPath))
{
    if (-not (Test-Path $nugetDir))
    {
        New-Item $nugetDir -ItemType Directory -ErrorAction SilentlyContinue
    }

    $nugetSourceUri = 'http://dist.nuget.org/win-x86-commandline/latest/nuget.exe'
    Invoke-WebRequest -Uri $nugetSourceUri -OutFile $nugetPath
}

# Retrieve InfinniPlatform version

$solutionPackagesConfig = Join-Path (Join-Path $solutionDir $solutionName) 'packages.config'
$platformVersion = (Select-Xml -Path $solutionPackagesConfig -XPath "//package[@id='InfinniPlatform.Sdk']").Node.version

# Check previous installation

$platformVersionMarker = Join-Path $solutionOutDir '.platformVersion'

$prevPlatformVersion = Get-Content -Path $platformVersionMarker -ErrorAction SilentlyContinue

if ($prevPlatformVersion -match $platformVersion)
{
    return
}

# Install InfinniPlatform package

$solutionPackagesDir = Join-Path $solutionDir 'packages'
& "$nugetPath" install 'InfinniPlatform' -Version $platformVersion -OutputDirectory $solutionPackagesDir -NonInteractive -Prerelease

# Copy InfinniPlatform files

$platformOutDir = Join-Path $solutionOutDir 'platform'
$platformPackage = Join-Path $solutionPackagesDir "InfinniPlatform.$platformVersion\lib\$framework\"

Remove-Item -Path $platformOutDir -Recurse -ErrorAction SilentlyContinue
Copy-Item -Path $platformPackage -Destination $platformOutDir -Exclude @( '*.ps1', '*references' ) -Recurse -ErrorAction SilentlyContinue

$platformReferences = Join-Path $platformPackage "InfinniPlatform.references"

if (Test-Path $platformReferences)
{
    Get-Content $platformReferences | Foreach-Object {
        if ($_ -match '^.*?\\lib(\\.*?){0,1}\\(?<path>.*?)$')
        {
            $item = Join-Path $platformOutDir $matches.path

            $itemParent = Split-Path $item

            if (-not (Test-Path $itemParent))
            {
                New-Item $itemParent -ItemType Directory | Out-Null
            }

            Copy-Item -Path (Join-Path $solutionPackagesDir $_) -Destination $item -Exclude @( '*.ps1', '*references' ) -Recurse -ErrorAction SilentlyContinue
        }
    }
}

# Copy InfinniPlatform.ServiceHost files

$serviceHostPackage = Join-Path $solutionPackagesDir "InfinniPlatform.ServiceHost.$platformVersion\lib\$framework\*"
Copy-Item -Path $serviceHostPackage -Destination $solutionOutDir -Recurse -ErrorAction SilentlyContinue

# Save installation number

Set-Content -Path $platformVersionMarker -Value $platformVersion -ErrorAction SilentlyContinue
