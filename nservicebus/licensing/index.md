---
title: Licensing
summary: Outlines license usage, management, and restrictions
component: core
reviewed: 2017-04-20
redirects:
 - nservicebus/licensing-limitations
 - nservicebus/licensing/licensing-limitations
 - nservicebus/licensing/license-management
 - nservicebus/license-management
related:
 - nservicebus/upgrades/release-policy
 - nservicebus/upgrades/support-policy
 - servicecontrol/license
 - serviceinsight/license
 - persistence/ravendb/licensing
---


## License details

See the [Licensing page](https://particular.net/licensing) for license specifics.


## License validity

partial: validity


## Throughput limitations

partial: limitations


## License management

There are several options available for installing the license file. 

While all of the options below work for NServiceBus, using the registry, ServiceControl utilities, or ServiceInsight are the only options that lets the other platform tools share the same license file. More information about this can be found in the
[ServiceControl licensing](/servicecontrol/license.md) and the  [ServiceInsight licensing](/serviceinsight/license.md) pages.

### Using the registry

partial: registry

partial: registry-caveats


partial: code

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


## Order of license detection

This section details where NServiceBus will look for license information, and in what order. For example, an expired license in `HKEY_CURRENT_USER` would overrule a valid license in `HKEY_LOCAL_MACHINE`. Note that these vary somewhat in older versions.

In order to find the license, NServiceBus will examine:

partial: order