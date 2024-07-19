---
title: Upgrade from ServiceControl.Plugin.NsbX.CustomChecks to NServiceBus.CustomChecks
summary: Instructions on how to upgrade CustomChecks Plugins to the new NServiceBus.CustomChecks package
reviewed: 2021-11-04
component: CustomChecks
isUpgradeGuide: true
ignoreSeoRecommendations: true
upgradeGuideCoreVersions:
 - 5
 - 6
 - 7
---


## Connecting to ServiceControl

The **NServiceBus.CustomChecks** package replaces the **ServiceControl.Plugin.Nsb5.CustomChecks** and **ServiceControl.Plugin.Nsb6.CustomChecks** packages. It also introduces a new version compatible with NServiceBus version 7.

To update, remove the deprecated package and install the NServiceBus.CustomChecks package.

```ps1
Uninstall-Package ServiceControl.Plugin.Nsb6.CustomChecks
Install-Package NServiceBus.CustomChecks -Version 2.0.0
```

## Configuration

The deprecated packages allowed configuration of the ServiceControl queue via a convention in which an application setting `ServiceControl/Queue` was picked up automatically. The new package requires explicit configuration. When upgrading, the following code needs to be added to the endpoint configuration code to retrieve the ServiceControl queue from the configuration file and pass it to the plugin.

```csharp
var endpointConfiguration = new EndpointConfiguration("MyEndpoint");

var setting = ConfigurationManager.AppSettings["ServiceControl/Queue"];

endpointConfiguration.ReportCustomChecksTo(
    serviceControlQueue: setting);
```
