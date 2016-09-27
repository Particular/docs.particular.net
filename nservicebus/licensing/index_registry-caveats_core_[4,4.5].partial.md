
#### NServiceBus 4.x versions prior to Version 4.5

When installed using the [Install-NServiceBusLicense](/nservicebus/operations/management-using-powershell.md) PowerShell commandlet and the `LicenseInstaller.exe` tool that comes with the NServiceBus install, in NServiceBus Version 4.0, the license file was stored in `HKLM\Software\NServiceBus\{Major.Minor}\License` and in Version 4.3, this location was moved to `HKLM\Software\ParticularSoftware\NServiceBus\License`.

In order to install the license file in HKCU (same location in Version 3.3), use the `-c` option on the `LicenseInstaller.exe`
