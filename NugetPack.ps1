# relative to script directory
$srcRoot = '.\src'                       

# relative to $srcRoot
[string[]] $buildFiles = 'Cassette.Nancy\Cassette.Nancy.csproj'
[string[]] $nuspecFiles = 'Cassette.Nancy\Cassette.Nancy.nuspec'
$versionFile = 'SharedAssemblyInfo.cs'

$buildConfiguration = 'Release'
$outputPath = "D:\Dropbox\Packages"

Import-Module BuildUtilities

$versionFile = Resolve-Path(Join-Path $srcRoot $versionFile)

$version = Get-Version $versionFile
  
New-Path $outputPath


#foreach($buildFile in $buildFiles)
#{
#  Invoke-Build (Resolve-Path(Join-Path $srcRoot $buildFile)) $buildConfiguration
#}

foreach($nuspecFile in $nuspecFiles)
{
  New-Package (Resolve-Path(Join-Path $srcRoot $nuspecFile)) $version $outputPath
}

Remove-Module BuildUtilities
