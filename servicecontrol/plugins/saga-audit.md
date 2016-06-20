---
title: SagaAudit Plugin
summary: Helps visualize and debug Sagas with ServiceInsight
tags:
- ServiceControl
---

DANGER: **For Development only**. This plugin will result in a significant increase in the load placed on ServiceControl. As such it should not be used in production.

The SagaAudit plugin enables the [Saga View feature in ServiceInsight](/serviceinsight/#the-saga-view). It is built specifically to help developers verify Saga logic during development. It does this by capturing Saga message behavior and changes in Saga data/state as the Saga is being processed. It then sends this information to a ServiceControl endpoint setup in the development environment.

NOTE: Saga Audit messages will not be sent to Service Control if an Exception is thrown during Saga processing.

## Implementation

The SagaAudit plugin captures the following information:

 * The incoming messages (including timeouts) that initiate change in the saga.
 * The outgoing messages that the saga sends.
 * A snapshot of the current saga data.
 * The saga state

All this information is sent to and stored in ServiceControl. Note that the saga audit data is transmitted to ServiceControl via a message and is serialized using the built in Json Serializer of NServiceBus.


## Impact on Service Control performance

This plugin results in an increase in load in several areas

 1. Endpoint load in order to capture the required information
 1. Network load due to the extra information sent to ServiceControl
 1. ServiceControl load in the areas of ingestion, correlation and data cleanup. 

The increase in load is proportional to size of the saga data multiplied by the number of messages the the saga receives. Since both these variables are dependent on the specific saga implementation it is not possible to give accurate predictions on the impact of this load in a production system.


## NuGets
 * NServiceBus Version 6.x: [ServiceControl.Plugin.NSb6.SagaAudit](https://www.nuget.org/packages/ServiceControl.Plugin.Nsb6.SagaAudit)
 * NServiceBus Version 5.x: [ServiceControl.Plugin.Nsb5.SagaAudit](https://www.nuget.org/packages/ServiceControl.Plugin.Nsb5.SagaAudit)
 * NServiceBus Version 4.x: [ServiceControl.Plugin.Nsb4.SagaAudit](https://www.nuget.org/packages/ServiceControl.Plugin.Nsb4.SagaAudit)
 * NServiceBus Version 3.x: Not Available


### Deprecated NuGet

If using the older version of the plugin, namely **ServiceControl.Plugin.SagaAudit** remove the package and replace it with the appropriate plugin based on the NServiceBus version. This package has been deprecated and unlisted.


## Removing the plugin from Production

If currently running the endpoint with the SagaAudit plugin in Production, do the following to remove it:

 1. Stop the endpoint
 1. Delete the SagaAudit plugin dll from the endpoint's bin directory. (Either `ServiceControl.Plugin.Nsb5.SagaAudit.dll` or `ServiceControl.Plugin.Nsb4.SagaAudit.dll`). In addition to that, if there is an automated deployment processes in place, ensure that this dll is no longer included.
 1. Restart the endpoint

Doing so will stop sending the saga state change messages to ServiceControl reducing message load to ServiceControl. Turn it back on if or when needed.


## Temporarily debugging sagas in Production

To visualize the saga in Production and the plugin is not already deployed, then add the Saga Audit plugin in the same location where the saga is running and restart the endpoint. Use ServiceInsight to view the visualization and when done, follow the steps above to remove the plugin and restart the service.