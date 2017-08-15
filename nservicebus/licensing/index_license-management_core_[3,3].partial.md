While all of the options below work for NServiceBus, using the registry, ServiceControl utilities, or ServiceInsight are the only options that lets the other platform tools share the same license file. More information about this can be found in the
[ServiceControl licensing](/servicecontrol/license.md) and the  [ServiceInsight licensing](/serviceinsight/license.md) pages.

### Using the registry

The license can be stored in a registry key called `[HKEYCURRENTUSER\Software\NServiceBus\{Major.Minor}\License]`.

To install the license in the registry, use one of these options:

* The `LicenseInstaller.exe` tool that comes with the NServiceBus install.
* The [Install-License](/nservicebus/operations/management-using-powershell.md) PowerShell commandlet.
* If the trial license has expired and running in debug mode, the endpoint shows a dialog that allows installing the license.


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