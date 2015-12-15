---
title: SagaAudit Plugin
summary: 'Helps you to visualize and debug Sagas with ServiceInsight.'
tags:
- ServiceControl
---

DANGER: **For Development only**. This plugin will result in a significant increase in the load placed on ServiceControl. As such it should not be used in production.

The SagaAudit plugin enabled Saga Visualization in ServiceInsight. It is built specifically for developers to help debug Sagas by capturing every state change that the saga undergoes.  It is optimized for capturing and recording large amounts of data in regards to each saga message.  This information enables the display of detailed saga data, behavior, and current status in the ServiceInsight Saga View. The plugin sends the relevant saga state information as messages to the ServiceControl queue whenever a saga state changes. This enables the Saga View to be highly detailed and up-to-date.


## Implementation

The SagaAudit plugin captures the following information:

 * The incoming messages (including timeouts) that initiate change in the saga.
 * The outgoing messages that the saga sends.
 * A snapshot of the current saga data.

All this information is sent to and stored in ServiceControl. Note that the saga data transmitted to ServiceControl is serialized via the built in Json Serializer of NServiceBus.

This results in an increase in load in several areas

 1. Endpoint load in order to capture the required information
 2. Network load due to the extra information sent to ServiceControl
 3. ServiceControl load in the areas of ingestion, correlation and data cleanup.  

The increase in load is proportional to size of the saga data multiplied by the number of messages the the saga receives. Since both these variables are dependent on the specific saga implementation it is not possible to give accurate predictions on the impact of this load in a production system.


## NuGets

 * NServiceBus Version 5.x: [ServiceControl.Plugin.Nsb5.SagaAudit](https://www.nuget.org/packages/ServiceControl.Plugin.Nsb5.SagaAudit)
 * NServiceBus Version 4.x: [ServiceControl.Plugin.Nsb4.SagaAudit](https://www.nuget.org/packages/ServiceControl.Plugin.Nsb4.SagaAudit)
 * NServiceBus Version 3.x: Not Available


### Deprecated NuGet

If you are using the older version of the plugin, namely **ServiceControl.Plugin.SagaAudit** please remove the package and replace it with the appropriate plugin based on your NServiceBus version. This package has been deprecated and unlisted.


## Removing the plugin from Production

If you are currently running your endpoint with the SagaAudit plugin in Production, do the following to remove it:

1. Stop your endpoint
2. Delete the SagaAudit plugin dll from the endpoint's bin directory. (Either `ServiceControl.Plugin.Nsb5.SagaAudit.dll` or `ServiceControl.Plugin.Nsb4.SagaAudit.dll`). In addition to that, if you have automated deployment processes in place, please ensure that this dll is no longer included.
3. Restart your endpoint

Doing so will stop sending the saga state change messages to ServiceControl reducing message load to ServiceControl. You can always turn it back on if or when needed.


## Temporarily debugging sagas in Production

If you wish to visualize your saga in Production and the plugin is not already deployed, then add the Saga Audit plugin in the same location where your saga is running and restart your endpoint. Use ServiceInsight to view the visualization and when done, follow the steps above to remove the plugin and restart your service. This approach is very similar for example, when you need to use Journaling to debug a message based system, where you turn it on when needed to collect the information you wish to see and then remove it from production.
