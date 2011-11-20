param([string] $folderName, [string] $sourcePath, [string] $targetPath)
$sourcePath = Join-Path $sourcePath $folderName
$targetPath = Join-Path $targetPath $folderName
if(Test-Path $targetPath) { Remove-Item $targetPath -Recurse -Force | Out-Null }
Copy-Item $sourcePath $targetPath -Recurse -Force
"Copied $sourcePath to $targetPath"
""
