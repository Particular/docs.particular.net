> [!NOTE]
> Depending on the operating system, the paths may be case sensitive.
>
> NServiceBus uses the [`Environment.GetFolderPath(SpecialFolder)`](https://docs.microsoft.com/en-us/dotnet/api/system.environment.getfolderpath) method to determine the locations of some paths on each OS.

### Code-first configuration

A license can be configured via code-first configuration API:

snippet: License

> [!NOTE]
> Licenses configured via code-first API take precendence over every other license source.

### Application-specific license location

A license located at `{AppDomain.CurrentDomain.BaseDirectory}/license.xml` will be automatically detected.

### User-specific license location

To install a license for all endpoints and Particular Service Platform applications run by a specific user, install the license file in the following location:

* Windows: `%LOCALAPPDATA%\ParticularSoftware\license.xml`
* Linux/macOS: `${XDG_DATA_HOME:-$HOME/.local/share}/ParticularSoftware/license.xml`
* macOS (.NET 8): `$HOME/Library/Application Support/ParticularSoftware/license.xml`

> [!NOTE]
> Ensure that the account under which the endpoint is running has permissions to access the user folder.

### Machine-wide license location

To install a license for all endpoints and Particular Service Platform applications on an entire machine, install the license file in the following location:

* Windows: `%PROGRAMDATA%\ParticularSoftware\license.xml`
* Linux/macOS: `/usr/share/ParticularSoftware/license.xml`

### Windows Registry

> [!WARNING]
> This option not available when targeting .NET Core.

Licenses stored in a registry key named `License` in the following registry locations are automatically detected:
* `HKEY_LOCAL_MACHINE\Software\ParticularSoftware`
* `HKEY_CURRENT_USER\Software\ParticularSoftware`

To install a license as a registry key, use the following steps:
* Start the [Registry Editor](https://technet.microsoft.com/en-us/library/cc755256.aspx).
* Go to `HKEY_LOCAL_MACHINE\Software\ParticularSoftware` or `HKEY_CURRENT_USER\Software\ParticularSoftware`.
* Create a new Multi-String Value (`REG_MULTI_SZ`) named `License`.
* Paste the contents of the license file.

> [!NOTE]
> If `HKEY_LOCAL_MACHINE` is the chosen license location, and the operating system is 64-bit, then repeat the import process for the `HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\ParticularSoftware` key to support 32-bit clients.

> [!NOTE]
> If the license is stored in `HKEY_CURRENT_USER`, NServiceBus processes must run as the user account used to add the license file to the registry in order to access the license.

It is safe to ignore any warnings regarding empty strings.

### Environment variable

The license can also be specified by setting the `PARTICULARSOFTWARE_LICENSE` environment variable containing the license text.

* Windows
  ```powershell
  $env:PARTICULARSOFTWARE_LICENSE = @"the license text goes here
  and is a multiline string"@
  ```
* Linux/macOS
  ```bash
  export PARTICULARSOFTWARE_LICENSE=`cat ./license.xml`
  export PARTICULARSOFTWARE_LICENSE="the license text goes here
  and is a multiline string"
  ```
