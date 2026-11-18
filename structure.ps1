
$root = (Get-Location).Path
$zip = Join-Path (Split-Path $root -Parent) "Alexandria-Source.zip"

$exclude = '\\(bin|obj|\.git|\.vs|TestResults|packages)\\'

$files = Get-ChildItem -Path $root -Recurse -File |
    Where-Object {
        $_.FullName -notmatch $exclude
    }

Compress-Archive -Path $files.FullName -DestinationPath $zip -Force

Write-Host "ZIP erstellt: $zip"
