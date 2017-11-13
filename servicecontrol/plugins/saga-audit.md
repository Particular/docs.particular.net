---
title: SagaAudit Plugin
summary: Helps visualize and debug Sagas with ServiceInsight
component: SagaAudit
versions: 'SagaAudit4:*;SagaAudit5:*;SagaAudit6:*'
reviewed: 2017-11-08
related:
 - nservicebus/sagas/saga-audit
---

WARNING: The following documentation describes deprecated packages ServiceControl.Plugin.Nsb5.SagaAudit and ServiceControl.Plugin.Nsb6.SagaAudit. To learn about the replacement package see [NServiceBus.SagaAudit](/nservicebus/sagas/saga-audit.md). To learn how to upgrade consult the [upgrade guide](/nservicebus/upgrades/nservicebus.sagaaudit.md).

DANGER: **For Development only**. This plugin will result in a significant increase in the load placed on ServiceControl. As such it should not be used in production.

The SagaAudit plugin enables the [Saga View feature in ServiceInsight](/serviceinsight/#the-saga-view). It is built specifically to help developers verify Saga logic during development. It does this by capturing Saga message behavior and changes in Saga data/state as the Saga is being processed. It then sends this information to a ServiceControl endpoint setup in the development environment.

NOTE: Saga audit messages will not be sent to ServiceControl if an Exception is thrown during Saga processing.


## Implementation

The SagaAudit plugin captures the following information:

 * The incoming messages (including timeouts) that initiate change in the saga.
 * The outgoing messages that the saga sends.
 * A snapshot of the current saga data.
 * The saga state

All this information is sent to and stored in ServiceControl. Note that the saga audit data is transmitted to ServiceControl via a message and is serialized using the built in JSON Serializer of NServiceBus.


## Impact on ServiceControl performance

This plugin results in an increase in load in several areas

 1. Endpoint load in order to capture the required information
 1. Network load due to the extra information sent to ServiceControl
 1. ServiceControl load in the areas of ingestion, correlation and data cleanup.

The increase in load is proportional to size of the saga data multiplied by the number of messages the the saga receives. Since both these variables are dependent on the specific saga implementation it is not possible to give accurate predictions on the impact of this load in a production system.


### Deprecated NuGet Packages

The following SagaAudit plugin packages have been deprecated and unlisted. If using one of these versions replace package references to use **NServiceBus.SagaAudit**.

- **ServiceControl.Plugin.SagaAudit**
- **ServiceControl.Plugin.Nsb5.SagaAudit**
- **ServiceControl.Plugin.Nsb6.SagaAudit**

## Configuration

partial: config


## Removing the plugin from Production

Removing the plugin before deployment to production depends on how the plugin is configured.

Disable plugin in production via config: 

 1. Stop the endpoint.
 1. Delete the SagaAudit plugin dll from the endpoint's bin directory. 
 1. If there is an automated deployment processes in place, ensure that this dll is no longer included.
 1. Restart the endpoint.

partial: remove-code-config

Disabling the SagaAudit plugin will stop sending the saga state change messages to ServiceControl reducing message load to ServiceControl. Turn it back on if or when needed.

## Temporarily debugging sagas in Production

To temporarily start visualizing a saga in production, the Saga Audit plugin can be deployed with the endpoint hosting the saga. Deploy the assembly at the same location as the endpoint is running, configure it and restart the endpoint. Use ServiceInsight to view the visualization and when done, follow the steps above to remove the plugin and restart the service.
