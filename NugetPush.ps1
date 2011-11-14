$srcRoot = '.\src'                     # relative to script directory
$versionFile = 'SharedAssemblyInfo.cs' # relative to $srcRoot
$outputPath = '.\build'                # relative to script directory
$scriptRoot = "$home\Dropbox\Scripts"

$version = . "$scriptRoot\Get-Version.ps1" (Join-Path $srcRoot $versionFile -Resolve)

. "$scriptRoot\Push-Project.ps1" Nancy.Cassette $srcRoot $version $outputPath
