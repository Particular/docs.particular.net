---
title: SagaAudit Plugin
summary: Helps visualize and debug Sagas with ServiceInsight
component: SagaAudit
reviewed: 2017-11-08
---

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

## Configuration

The SagaAudit plugin is enabled via

partial: SagaAuditNew_Enable

In order to not run it in Production environments at all times it is advised to enable it conditionally, based on an environment variable or configuration setting. To temporarily start visualizing a saga in production, change the setting and restart the endpoint. Use ServiceInsight to view the visualization and when done, change the configuration back and restart the service.


## Custom serialization

The SagaAudit plugin serializes the saga data objects using a simple json serializer. This serializer should be fine for most use cases. It handles well all primitive types (including `TimeSpan` and `DateTime`), their nullable variants as well as nested objects. However, if more sophisticated mechanism is required, the serialization method can be provided by the user

partial: SagaAuditNew_CustomSerialization
