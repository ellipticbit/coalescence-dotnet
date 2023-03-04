param (
)

[System.Net.ServicePointManager]::SecurityProtocol = [System.Net.SecurityProtocolType]::Tls12

$year = [System.DateTime]::Now.Year
$BuildMajor = ([int]$env:BUILD_MAJOR)
$BuildMinor = ([int]$env:BUILD_MINOR)
$BuildNumber = ([int]$env:BUILD_NUMBER_REQUEST) + 1
Invoke-WebRequest "https://gitlab.com/api/v4/projects/$($env:CI_PROJECT_ID)/variables/BUILD_NUMBER_REQUEST" -Headers @{"PRIVATE-TOKEN"=$env:CI_API_TOKEN} -Body @{value=$BuildNumber} -ContentType "application/x-www-form-urlencoded" -Method "PUT" -UseBasicParsing

$appVer = '{0}.{1}.{2}' -f $BuildMajor, $BuildMinor, $BuildNumber
$copyright = 'Copyright © EllipticBit, LLC. {0}, All Rights Reserved.' -f $year

Get-ChildItem -Path .\Request\ -Filter *.csproj -Recurse -File | ForEach-Object {
    [string]$filename = $_.FullName
    [xml]$filexml = Get-Content -Path $_.FullName -Encoding UTF8

    Try
    {
        $filexml.Project.PropertyGroup.Copyright = $copyright
        $filexml.Project.PropertyGroup.Version = $appVer
        $filexml.Project.PropertyGroup.FileVersion = $appVer
    }
    Catch
    {
        Write-Host "No Version or FileVersion properties found in project: $($filename)"
    }

    $filexml.InnerXml | Out-File $_.FullName -Encoding UTF8
}

Write-Host "Build Version: $($appVer)"
