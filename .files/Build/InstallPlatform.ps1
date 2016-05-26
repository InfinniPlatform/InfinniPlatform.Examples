<#
.Synopsis
	Installs InfinniPlatform to the solution output directory.
#>
param
(
	[Parameter(HelpMessage = "Name of the solution.")]
	[String] $SolutionName,

	[Parameter(HelpMessage = "Path to the solution directory.")]
	[String] $SolutionDir,

	[Parameter(HelpMessage = "Path to the solution output directory.")]
	[String] $SolutionOutDir
)

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

$solutionPackagesConfig = Join-Path (Join-Path $SolutionDir $SolutionName) 'packages.config'
$platformVersion = (Select-Xml -Path $solutionPackagesConfig -XPath "//package[@id='InfinniPlatform.Sdk']").Node.version

# Install InfinniPlatform package

$solutionPackagesDir = Join-Path $SolutionDir 'packages'
& "$nugetPath" install 'InfinniPlatform' -Version $platformVersion -OutputDirectory $solutionPackagesDir -NonInteractive

# Copy InfinniPlatform files

Copy-Item -Path "$solutionPackagesDir\InfinniPlatform.$platformVersion\lib\net45\*" -Destination $SolutionOutDir -Recurse -ErrorAction SilentlyContinue
Copy-Item -Path "$solutionPackagesDir\InfinniPlatform.$platformVersion\content\metadata" -Destination "$SolutionOutDir\content\$SolutionName\metadata" -Recurse -ErrorAction SilentlyContinue
