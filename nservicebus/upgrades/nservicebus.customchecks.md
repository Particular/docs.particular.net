---
title: Upgrade from ServiceControl.Plugin.NsbX.CustomChecks to NServiceBus.CustomChecks
summary: Instructions on how to upgrade CustomChecks Plugins to the new NServiceBus.CustomChecks package
reviewed: 2017-11-08
component: ServiceControl
related:
 - servicecontrol/plugins
 - monitoring/custom-checks/legacy
isUpgradeGuide: true
ignoreSeoRecommendations: true
upgradeGuideCoreVersions:
 - 5
 - 6
 - 7
---


## Connecting to ServiceControl

The NserviceBus.CustomChecks package replaces the **ServiceControl.Plugin.Nsb5.CustomChecks** and **ServiceControl.Plugin.Nsb6.CustomChecks** packages. It also introduces a new version compatible with NServiceBus Version 7.

To update, remove the deprecated package and install the NServiceBus.CustomChecks package.

snippet: NSBCustomChecks_Upgrade_InstallPackage

## Configuration

The deprecated packages allowed configuration of ServiceControl queue via a convention in which an application setting `ServiceControl/Queue` was picked up automatically. The new package requires explicit configuration. When upgrading, the following code needs to be added to the endpoint setup code to retrieve the ServiceControl queue from the configuration file and pass it to the plugin.

snippet: NSBCustomChecks_Upgrade_Configure
