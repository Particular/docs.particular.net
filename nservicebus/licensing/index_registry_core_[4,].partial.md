#### NServiceBus PowerShell cmdlet

The [NServiceBus PowerShell Module](/nservicebus/operations/management-using-powershell.md) includes a cmdlet for importing the Platform License into the `HKEY_LOCAL_MACHINE` root key of the registry. 

For 64-bit operating systems, the license is stored under "license" value in both the `HKEY_LOCAL_MACHINE\Software\ParticularSoftware` and `HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\ParticularSoftware` registry keys.


#### PowerShell

* Open an administrative PowerShell console.
* Change the current working directory to where the license.xml file is.
* Run the following script:

```ps
$content = Get-Content license.xml | Out-String
New-Item -Path HKLM:\Software\ParticularSoftware -Force 
Set-ItemProperty -Path HKLM:\Software\ParticularSoftware -Name License -Force -Value $content
```

NOTE: For 64-bit operating systems, this process should be done in both the PowerShell and PowerShell(x86) consoles. This will ensure the license is imported into both the 32-bit and 64-bit registry keys.


#### Registry Editor

 * Start the [Registry Editor](https://technet.microsoft.com/en-us/library/cc755256.aspx).
 * Go to `HKEY_LOCAL_MACHINE\Software\ParticularSoftware` or `HKEY_CURRENT_USER\Software\ParticularSoftware`.
 * Create a new Multi-String Value (`REG_MULTI_SZ`) named `License`.
 * Paste the contents of the license file.

NOTE: If `HKEY_LOCAL_MACHINE` is the chosen license location, and the operating system is 64-bit, then repeat the import process for the `HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\ParticularSoftware` key to support 32-bit clients.

NOTE: If the license is stored in `HKEY_CURRENT_USER`, NServiceBus processes must run as the user account used to add the license file to the registry in order to access the license.

It is safe to ignore any warnings regarding empty strings.