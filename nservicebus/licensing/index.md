---
title: Licensing
summary: Outlines license usage, management, and restrictions
component: core
reviewed: 2016-09-27
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
 - nservicebus/ravendb/licensing
---


## License details

See [Particular Licensing](https://particular.net/licensing) for license specifics.

## License validity

partial: validity

## Throughput limitations

partial: limitations


## License Management

partial: code


### Using the Registry

NOTE: Using the [NServiceBus PowerShell Module](/nservicebus/operations/management-using-powershell.md) is the preferred and simplest method of adding the license file. 

Using the registry allows all platform tools (endpoints, [ServiceControl](/servicecontrol/license.md), and [ServiceInsight](/serviceinsight/license.md)) to access the same license.

partial: registry

partial: registry-caveats


### Using a License sub-directory

A file located at `[AppDomain.CurrentDomain.BaseDirectory]/License/License.xml` will be automatically detected.


### Using the app.config settings

It is possible to specify the license in `app.config`:

 - Use the key `NServiceBus/LicensePath` to specify the path where NServiceBus looks for the license. For example:

```xml
<appSettings>
  <add key="NServiceBus/LicensePath"
       value="C:\NServiceBus\License\License.xml" />
</appSettings>
```
 - Use the key `NServiceBus/License` to transfer the actual XML-encoded contents of the license. For example:

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