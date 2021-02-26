---
title: Upgrade from ServiceControl.Plugin.NsbX.Heartbeat to NServiceBus.Heartbeat
summary: Instructions on how to upgrade Heartbeat Plugins to the new NServiceBus.Heartbeat package
reviewed: 2019-09-03
component: Heartbeats
related:
 - servicecontrol/plugins
 - monitoring/heartbeats/legacy
isUpgradeGuide: true
ignoreSeoRecommendations: true
upgradeGuideCoreVersions:
 - 5
 - 6
 - 7
---


## Connecting to ServiceControl

The **NServiceBus.Heartbeat** package replaces the **ServiceControl.Plugin.Nsb5.Heartbeat** and **ServiceControl.Plugin.Nsb6.Heartbeat** packages. It also introduces a new version compatible with NServiceBus version 7.

To update, remove the deprecated package and install the NServiceBus.Heartbeat package.

snippet: NSBHeartbeat_Upgrade_InstallPackage

## Configuration

The deprecated packages allowed configuration of the ServiceControl queue via a convention in which an application setting `ServiceControl/Queue` was picked up automatically. The new package requires explicit configuration. When upgrading, the following code needs to be added to the endpoint configuration code to retrieve the ServiceControl queue from the configuration file and pass it to the plugin.

snippet: NSBHeartbeat_Upgrade_Configure

## Heartbeats feature made internal

The deprecated packages contained a public feature class called `Heartbeats`. This feature was enabled by default and exposed publicly so that it could be disabled using the features API. 

snippet: NSBHeartbeat_Upgrade_Disable

This feature is no longer enabled by default and requires explicit configuration (above) in order to activate. The feature class itself has been made internal and any code referencing it to disable the feature can be removed. 
