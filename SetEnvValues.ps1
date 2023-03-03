param (
)

[string]$file = Get-Content -Path ".\NuGet.config" -Encoding UTF8
$file = $file.Replace("%EB_NUGET_TOKEN%", $env:EB_NUGET_TOKEN);
$file | Out-File ".\NuGet.config" -Encoding UTF8
