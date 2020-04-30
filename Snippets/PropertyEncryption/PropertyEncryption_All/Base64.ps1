# startcode Base64-Powershell
Add-Type -AssemblyName System.Security

[Reflection.Assembly]::LoadWithPartialName("System.Security")
$rijndael = new-Object System.Security.Cryptography.RijndaelManaged
$rijndael.GenerateKey()
Write-Host([Convert]::ToBase64String($rijndael.Key))
$rijndael.Dispose()

# endcode