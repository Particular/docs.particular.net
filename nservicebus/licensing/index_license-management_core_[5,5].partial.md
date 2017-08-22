While all of the options below work for NServiceBus, using the registry, ServiceControl utilities, or ServiceInsight are the only options that lets the other platform tools share the same license file. More information about this can be found in the
[ServiceControl licensing](/servicecontrol/license.md) and the  [ServiceInsight licensing](/serviceinsight/license.md) pages.

### Using the registry


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


### Using the configuration API

It is possible to specify the license to use in the configuration code:

snippet: License


### Using a license subdirectory

A file located at `[AppDomain.CurrentDomain.BaseDirectory]/License/License.xml` will be automatically detected.


### Using app.config appSettings

It is possible to specify the license in `app.config`:

- Use the key `NServiceBus/LicensePath` to specify the path where NServiceBus looks for the license:

```xml
<appSettings>
  <add key="NServiceBus/LicensePath"
       value="C:\NServiceBus\License\License.xml" />
</appSettings>
```
 - Use the key `NServiceBus/License` to store the XML-encoded contents of the license directly in `app.config`:

```xml
<appSettings>
  <add key="NServiceBus/License" value="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;
license id=&quot;1222e1d1-2222-4a46-b1c6-943c442ca710&quot; expiration=&quot;2013-11-30T00:00:00.0000000
&quot; type=&quot;Standard&quot; LicenseType=&quot;Standard&quot; LicenseVersion=&quot;4.0
&quot; MaxMessageThroughputPerSecond=&quot;Max&quot; WorkerThreads=&quot;Max
&quot; AllowedNumberOfWorkerNodes=&quot;Max&quot;&gt;
. . .
&lt;/license&gt;" />
</appSettings>
```


### Order of license detection

This section details where NServiceBus will look for license information, and in what order. For example, an expired license in `HKEY_CURRENT_USER` would overrule a valid license in `HKEY_LOCAL_MACHINE`. Note that these vary somewhat in older versions.

In order to find the license, NServiceBus will examine:

| Location                                                                          | Notes |
|-----------------------------------------------------------------------------------|:-----:|
| `HKEY_CURRENT_USER\Software\ParticularSoftware\License`                           |       |
| `HKEY_LOCAL_MACHINE\Software\ParticularSoftware\License`                          |       |
| `HKEY_LOCAL_MACHINE\Software\Wow6432Node\ParticularSoftware\License`              |   1   |
| License XML defined by `NServiceBus/License` appSetting                           |       |
| File path configured through `NServiceBus/LicensePath` appSetting                 |       |
| File located at `{AppDomain.CurrentDomain.BaseDirectory}\NServiceBus\License.xml` |       |
| File located at `{AppDomain.CurrentDomain.BaseDirectory}\License\License.xml`     |       |

**Notes:**

 1. The `Wow6432Node` registry keys are only accessed if running a 32-bit host on a 64-bit OS.