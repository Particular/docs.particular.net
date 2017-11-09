---
title: Upgrade from ServiceControl.Plugin.NsbX.SagaAudit to NServiceBus.SagaAudit
summary: Instructions on how to upgrade SagaAudit Plugins to the new NServiceBus.SagaAudit package
reviewed: 2017-11-08
component: ServiceControl
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

The NserviceBus.SagaAudit package replaces the **ServiceControl.Plugin.Nsb5.SagaAudit** and **ServiceControl.Plugin.Nsb6.SagaAudit** packages. It also introduces a new version compatible with NServiceBus Version 7.

To update, remove the deprecated package and install the NServiceBus.SagaAudit package.

## Configuration

The location of the ServiceControl queue must be specified via extensions to the endpoint configuration.

#### SagaAudit

TODO: Place snippet here
