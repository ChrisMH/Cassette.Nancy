$srcPath = ".\src\"
$buildPath = ".\build\"
$versionFile = $srcPath + "SharedAssemblyInfo.cs"
$nuget = ".\tools\nuget\nuget.exe"

if (test-path $buildPath) { remove-item -Recurse -Force $buildPath }
mkdir $buildPath | out-null

get-content $versionFile | where-object { $_ -match '^\[\s*assembly:\s*AssemblyVersion\s*\(\s*\"(?<version>[\d\.]*)\"\s*\)\s*\]' } | out-null

#Nancy.Cassette
$packageName = "Nancy.Cassette"
$packFile = $srcPath + $packageName + "\" + $packageName + ".csproj";
&$nuget pack $packFile -Version $matches.version -Build -Properties Configuration=Release -OutputDirectory $buildPath
