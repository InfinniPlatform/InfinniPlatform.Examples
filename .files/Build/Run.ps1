<#
.Synopsis
	Builds and runs the solution based on InfinniPlatform.
#>
param
(
	[Parameter(HelpMessage = "Name of the solution.")]
	[String] $SolutionName = 'InfinniPlatform.Northwind',

	[Parameter(HelpMessage = "Path to the solution output directory.")]
	[String] $SolutionOutDir = 'Assemblies',

	[Parameter(HelpMessage = "Solution build mode (Debug or Release).")]
	[String] $SolutionConfig = 'Debug'
)

# Install NuGet

$nugetPath = Join-Path (Join-Path $env:ProgramData 'NuGet') 'nuget.exe'

if (-not (Test-Path $nugetPath))
{
	$nugetSourceUri = 'http://dist.nuget.org/win-x86-commandline/latest/nuget.exe'
	Invoke-WebRequest -Uri $nugetSourceUri -OutFile $nugetPath
}

# Retrieve InfinniPlatform version

$solutionPackages = Join-Path $SolutionName 'packages.config'
$platformVersion = (Select-Xml -Path $solutionPackages -XPath "//package[@id='InfinniPlatform.Sdk']").Node.version

# Install InfinniPlatform package

& "$nugetPath" install 'InfinniPlatform' -Version $platformVersion -OutputDirectory 'packages' -NonInteractive

# Restore solution packages

& "$nugetPath" restore "$SolutionName.sln" -NonInteractive

# Build solution

& "${Env:ProgramFiles(x86)}\MSBuild\14.0\bin\msbuild.exe" "$SolutionName.sln" /p:Configuration=$SolutionConfig /verbosity:quiet /consoleloggerparameters:ErrorsOnly

# Copy InfinniPlatform files

Copy-Item -Path "packages\InfinniPlatform.$platformVersion\lib\net45\*" -Destination $SolutionOutDir -ErrorAction SilentlyContinue
Copy-Item -Path "packages\InfinniPlatform.$platformVersion\content\metadata" -Destination "$SolutionOutDir\content\$SolutionName\metadata" -Recurse -ErrorAction SilentlyContinue

# Run solution

Set-Location -Path $SolutionOutDir

& ".\InfinniPlatform.ServiceHost.exe"
