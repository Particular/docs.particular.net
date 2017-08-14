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
$containerName = "endpoints"
$container = Get-AzureStorageContainer -Name $containerName -Context $context -ErrorAction SilentlyContinue 
if (!$container)
{
    Write-Host "Creating '$containerName' container for uploaded endpoints" -ForegroundColor Cyan
    New-AzureStorageContainer -Name $containerName -Permission Off -Context $context
}

ls -path $zipsDir -file -recurse | Set-AzureStorageBlobContent -Container $containerName -Context $context -Force

Write-Host "Endpoints deployed to storage account " $context.StorageAccountName -ForegroundColor Cyan
