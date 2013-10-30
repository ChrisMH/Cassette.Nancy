$srcRoot = '.\src'                     # relative to script directory
$versionFile = 'SharedAssemblyInfo.cs' # relative to $srcRoot
$outputPath = "D:\Dropbox\Packages"

Import-Module NugetUtilities

$version = Get-Version (Join-Path $srcRoot $versionFile)

Push-Project Cassette.Nancy $srcRoot $version $outputPath
