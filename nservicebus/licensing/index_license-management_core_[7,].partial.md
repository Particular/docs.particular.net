### Application specific license location

A license located at `{AppDomain.CurrentDomain.BaseDirectory}/license.xml` will be automatically detected.


### Machine wide license locations

Licenses can be shared across all endpoints and Particular Service Platform applications by placing them into one of the following locations:
* `{SpecialFolder.LocalApplicationData}\ParticularSoftware\license.xml`
* `{SpecialFolder.CommonApplicationData}\ParticularSoftware\license.xml`

For Windows, these locations are:
* `%LOCALAPPDATA%\ParticularSoftware\license.xml`
* `%PROGRAMDATA%\ParticularSoftware\license.xml`

For Linux/macOS, these locations are:
* `${XDG_DATA_HOME:-$HOME/.local/share}/ParticularSoftware`
* `/usr/share/ParticularSoftware/license.xml`

Note: Depending on the operating system, the paths may be case sensitive.


### Code first configuration

A license can be configured via code first configuration API:

snippet: License


### Application configuration file

WARNING: This option is only available when targeting the full .NET Framework.

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

### Windows Registry

WARNING: This option is only available when targeting the full .NET Framework.

Licenses stored in a registry key named `License` in the following registry locations are automatically detected:
* `HKEY_LOCAL_MACHINE\Software\ParticularSoftware`
* `HKEY_CURRENT_USER\Software\ParticularSoftware`

To install a license as a registry key, use the following steps:
* Start the [Registry Editor](https://technet.microsoft.com/en-us/library/cc755256.aspx).
* Go to `HKEY_LOCAL_MACHINE\Software\ParticularSoftware` or `HKEY_CURRENT_USER\Software\ParticularSoftware`.
* Create a new Multi-String Value (`REG_MULTI_SZ`) named `License`.
* Paste the contents of the license file.

NOTE: If `HKEY_LOCAL_MACHINE` is the chosen license location, and the operating system is 64-bit, then repeat the import process for the `HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\ParticularSoftware` key to support 32-bit clients.

NOTE: If the license is stored in `HKEY_CURRENT_USER`, NServiceBus processes must run as the user account used to add the license file to the registry in order to access the license.

It is safe to ignore any warnings regarding empty strings.
