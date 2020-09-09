{{NOTE:
Depending on the operating system, the paths may be case sensitive.

NServiceBus uses the [`Environment.GetFolderPath(SpecialFolder)`](https://docs.microsoft.com/en-us/dotnet/api/system.environment.getfolderpath) method to determine the locations of some paths on each OS.
}}

### Code-first configuration

A license can be configured via code-first configuration API:

snippet: License

NOTE: Licenses configured via code-first API take precendence over every other license source.


### Application-specific license location

A license located at `{AppDomain.CurrentDomain.BaseDirectory}/license.xml` will be automatically detected.


### User-specific license location

To install a license for all endpoints and Particular Service Platform applications run by a specific user, install the license file to `{SpecialFolder.LocalApplicationData}\ParticularSoftware\license.xml`.

This location can be expressed using environment variables on Windows, or a bash expression on Linux/macOS:

* Windows: `%LOCALAPPDATA%\ParticularSoftware\license.xml`
* Linux/macOS: `${XDG_DATA_HOME:-$HOME/.local/share}/ParticularSoftware/license.xml`


### Machine-wide license location

To install a license for all endpoints and Particular Service Platform applications on an entire machine, install the license file to `{SpecialFolder.CommonApplicationData}\ParticularSoftware\license.xml`.

This location can be expressed using environment variables on Windows, or as a literal path on Linux/macOS.

* Windows: `%PROGRAMDATA%\ParticularSoftware\license.xml`
* Linux/macOS: `/usr/share/ParticularSoftware/license.xml`


### Application configuration file

WARNING: This option not available when targeting .NET Core.

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

WARNING: This option not available when targeting .NET Core.

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
