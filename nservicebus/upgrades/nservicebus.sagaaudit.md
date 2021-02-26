---
title: Upgrade from ServiceControl.Plugin.NsbX.SagaAudit to NServiceBus.SagaAudit
summary: Instructions on how to upgrade SagaAudit Plugins to the new NServiceBus.SagaAudit package
reviewed: 2019-09-03
component: SagaAudit
related:
 - servicecontrol/plugins
 - servicecontrol/plugins/saga-audit
isUpgradeGuide: true
ignoreSeoRecommendations: true
upgradeGuideCoreVersions:
 - 5
 - 6
 - 7
---


## Connecting to ServiceControl

The **NServiceBus.SagaAudit** package replaces the **ServiceControl.Plugin.Nsb5.SagaAudit** and **ServiceControl.Plugin.Nsb6.SagaAudit** packages. It also introduces a new version compatible with NServiceBus version 7.

To update, remove the deprecated package and install the NServiceBus.SagaAudit package.

snippet: NSBSagaAudit_Upgrade_InstallPackage

## Configuration

The deprecated packages allowed configuration of the ServiceControl queue via a convention in which an application setting `ServiceControl/Queue` was picked up automatically. The new package requires explicit configuration. When upgrading, the following code needs to be added to the endpoint configuration code to retrieve the ServiceControl queue from the configuration file and pass it to the plugin.

snippet: NSBSagaAudit_Upgrade_Configure
