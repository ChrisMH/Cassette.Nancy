$srcRoot = '.\src'                     # relative to script directory
$versionFile = 'SharedAssemblyInfo.cs' # relative to $srcRoot
$outputPath = '.\build'                # relative to script directory
$scriptRoot = "$home\Dropbox\Scripts"

. "$scriptRoot\New-Path.ps1" $outputPath

$version = . "$scriptRoot\Get-Version.ps1" (Join-Path $srcRoot $versionFile -Resolve)

. "$scriptRoot\Pack-Project.ps1" Cassette.Nancy $srcRoot $version $outputPath

