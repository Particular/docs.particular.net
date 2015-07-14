$baseDir = Split-Path (Resolve-Path $MyInvocation.MyCommand.Path)

# Packaging

$zipsDir = $baseDir + "\_zips";
Write-Host "Packaging endpoints as zip files..."

if ((Test-Path $zipsDir) -eq $true)
{
    Get-ChildItem -Path $zipsDir -Include *.* -Recurse | foreach { $_.Delete()}
}
else
{
    New-Item $zipsDir -ItemType Directory -Force
}

[Reflection.Assembly]::LoadWithPartialName( "System.IO.Compression.FileSystem")

$locationTemplate = "$baseDir\{0}\bin\debug"
$destinationTemplate = "$zipsDir\{0}.zip"

$location = $locationTemplate -f "Sender"
$destination = $destinationTemplate -f "Sender"
[System.IO.Compression.ZipFile]::CreateFromDirectory($location, $destination)

$location = $locationTemplate -f "Receiver"
$destination = $destinationTemplate -f "Receiver"
[System.IO.Compression.ZipFile]::CreateFromDirectory($location, $destination)

Write-Host "Zips are created at $zipsDir location" -ForegroundColor Cyan

# Deployment

Write-Host "Deploying endpoints..." -ForegroundColor Cyan

$context = New-AzureStorageContext -Local
ls -path $zipsDir -file -recurse | Set-AzureStorageBlobContent -Container "endpoints" -Context $context -Force
    
Write-Host "Endpoints deployed to storage account " $context.StorageAccountName -ForegroundColor Cyan
