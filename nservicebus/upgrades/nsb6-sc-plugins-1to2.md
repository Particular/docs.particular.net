---
title: NServiceBus Version 6 ServiceControl Plugins Upgrade Version 1 to 2
summary: Instructions on how to upgrade NServiceBus version 6 ServiceControl Plugins Version 1 to 2.
reviewed: 2016-12-07
component: ServiceControl
related:
 - servicecontrol/plugins
 - monitoring/heartbeats/legacy
 - monitoring/custom-checks/legacy
 - servicecontrol/plugins/saga-audit
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
---


## Connecting to ServiceControl

Version 2 of the ServiceControl plugins changes the way that the plugins connect to ServiceControl. The plugins no longer derive a ServiceControl queue name from the Error/Audit queues. Additional configuration is required to specify the location of the ServiceControl queue. 


### Configuration File

The location of the ServiceControl queue can be specified once for all plugins in via an `appSetting` in the endpoint configuration file.

snippet: sc-plugin-queue-config


### Code

The location of the ServiceControl queue can be specified via plugin-specific extensions to the endpoint configuration.


#### Heartbeats

snippet: Heartbeats_Configure_ServiceControl


#### CustomChecks

snippet: CustomCheck_Configure_ServiceControl


#### SagaAudit

snippet: SagaAudit_Configure_ServiceControl