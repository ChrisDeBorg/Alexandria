
$files = Get-ChildItem -Path . -Recurse -File |
    Where-Object {
        $_.FullName -notmatch '[\\/](bin|obj|\.git|\.vs)[\\/]'
    }

$files |
    Select-Object -ExpandProperty FullName |
    Set-Content -Path ".\filelist.txt" -Encoding UTF8