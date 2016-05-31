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
	[String] $framework = 'net45'
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

# Install InfinniPlatform package

$solutionPackagesDir = Join-Path $solutionDir 'packages'
& "$nugetPath" install 'InfinniPlatform' -Version $platformVersion -OutputDirectory $solutionPackagesDir -NonInteractive -Prerelease -Verbosity detailed

# Copy InfinniPlatform files

$platformPackage = Join-Path $solutionPackagesDir "InfinniPlatform.$platformVersion"
Copy-Item -Path (Join-Path $platformPackage "lib\$framework\*") -Destination $solutionOutDir -Recurse -ErrorAction SilentlyContinue
Copy-Item -Path (Join-Path $platformPackage "content\metadata") -Destination (Join-Path $solutionOutDir "content\$solutionName\metadata") -Recurse -ErrorAction SilentlyContinue

# Copy InfinniPlatform references

$platformReferences = Join-Path $platformPackage "lib\$framework\references.lock"

if (Test-Path $platformReferences)
{
	Get-Content $platformReferences | Foreach-Object {
		if ($_ -match '^.*?\\lib\\.*?\\(?<path>.*?)$')
		{
			$item = Join-Path "$solutionOutDir" $matches.path

			$itemParent = Split-Path $item

			if (-not (Test-Path $itemParent))
			{
				New-Item $itemParent -ItemType Directory
			}

			Copy-Item -Path (Join-Path $solutionPackagesDir $_) -Destination $item -Recurse -ErrorAction SilentlyContinue
		}
	}
}