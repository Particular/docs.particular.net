---
title: Upgrade from ServiceControl.Plugin.NsbX.Heartbeat to NServiceBus.Heartbeat
summary: Instructions on how to upgrade Heartbeat Plugins to the new NServiceBus.Heartbeat package
reviewed: 2017-11-08
component: ServiceControl
related:
 - servicecontrol/plugins
 - servicecontrol/plugins/heartbeat
isUpgradeGuide: true
ignoreSeoRecommendations: true
upgradeGuideCoreVersions:
 - 5
 - 6
 - 7
---


## Connecting to ServiceControl

The NserviceBus.Heartbeat package replaces the **ServiceControl.Plugin.Nsb5.Heartbeat** and **ServiceControl.Plugin.Nsb6.Heartbeat** packages. It also introduces a new version compatible with NServiceBus Version 7.

To update, remove the deprecated package and install the NServiceBus.Heartbeat package.

snippet: NSBHeartbeat_Upgrade_InstallPackage

## Configuration

The deprecated packages allowed configuration of ServiceControl queue via a convention in which an application setting `ServiceControl/Queue` was picked up automatically. The new package requires explicit configuration. When upgrading, the following code needs to be added to the endpoint setup code to retrieve the ServiceControl queue from the configuration file and pass it to the plugin.

snippet: NSBHeartbeat_Upgrade_Configure
