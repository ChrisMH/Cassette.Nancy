param([string] $folderName, [string] $sourcePath, [string] $targetPath)
$sourcePath = Join-Path $sourcePath $folderName


#if(Test-Path "$targetPath\$folderName") { Remove-Item "$targetPath\$folderName" -Recurse -Force | Out-Null }
Copy-Item $sourcePath $targetPath -Recurse -Force
