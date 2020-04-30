# startcode Hex-Powershell

Add-Type -AssemblyName System.Security

[Reflection.Assembly]::LoadWithPartialName("System.Security")
$rijndael = new-Object System.Security.Cryptography.RijndaelManaged
$rijndael.GenerateKey()
Write-Host([System.BitConverter]::ToString($rijndael.Key).Replace("-", "").ToLowerInvariant())
$rijndael.Dispose()

# endcode