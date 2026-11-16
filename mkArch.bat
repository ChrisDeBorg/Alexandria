$files = Get-ChildItem -Recurse -File 
    Where-Object {
        $_.FullName -notmatch '\\(bin|obj|\.git)\\' -and
        $_.Name -notlike 'appsettings*.json' -and
        $_.Name -ne '*.csproj.user'
    }
	
Compress-Archive -Path $files.FullName `
    -DestinationPath "..\Alexandria-source.zip" -Force