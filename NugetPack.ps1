$srcRoot = '.\src'                     # relative to script directory
$versionFile = 'SharedAssemblyInfo.cs' # relative to $srcRoot
$outputPath = "$home\Dropbox\Packages"
$scriptRoot = "$home\Dropbox\Scripts"

Import-Module NugetUtilities

$version = Get-Version (Join-Path $srcRoot $versionFile)

Pack-Project Cassette.Nancy $srcRoot $version $outputPath
