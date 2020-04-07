---
title: SagaAudit Plugin
summary: For visualizing and debugging sagas with ServiceInsight
component: SagaAudit
versions: 'SagaAudit:*'
reviewed: 2019-09-03
---

WARN: This plugin will result in a increase in the load placed on ServiceControl and the endpoint it is installed in. It should only be used if the environment is able to keep up with the increased load.

The SagaAudit plugin enables the [Saga View feature in ServiceInsight](/serviceinsight/#the-saga-view). 

![SagaAudit View in ServiceInsight](saga-audit-screenshot.png)

It is built specifically to help developers verify saga logic during development. It does this by capturing saga message behavior and changes in saga data/state as the saga is being processed. It then sends this information to a ServiceControl instance set up in the development environment. The information is available in the *Saga* view in ServiceInsight.


NOTE: Saga audit messages are only sent to ServiceControl when a message is processed successfully.


## Implementation

The SagaAudit plugin captures the following information:

 * The incoming messages (including timeouts) that initiate change in the saga
 * The outgoing messages that the saga sends
 * A snapshot of the current saga state

```mermaid
graph LR
subgraph Endpoint
Auditing
SagaAudit[Saga Audit]
end
	
SagaAudit -- Saga Change<br>Audit Data --> SCQ[ServiceControl<br>Input Queue]
	
Auditing -- Message<br>Audit Data --> AuditQ[audit<br>queue]

AuditQ --> ServiceControl
	
SCQ --> ServiceControl
```

All this information is sent to and stored in ServiceControl. Note that the saga state audit data is transmitted to ServiceControl via a separate message and is serialized using the built in JSON Serializer of NServiceBus.


## Impact on ServiceControl performance

This plugin results in an increase in load in several areas:

 1. Endpoint load in order to capture the required information
 1. Network load due to the extra information sent to ServiceControl
 1. ServiceControl load in the areas of ingestion, correlation and data cleanup

The increase in load is proportional to size of the saga data multiplied by the number of messages the the saga receives. Since both these variables are dependent on the specific saga implementation it is not possible to give accurate predictions on the impact of this load in a production system.

## Configuration

The SagaAudit plugin is enabled via:

snippet: SagaAuditNew_Enable

In order to avoid enabling the SagaAudit plugin in production environments, it is advised to enable it conditionally based on an environment variable or configuration setting. To temporarily start visualizing a saga in production, change the setting and restart the endpoint. Use ServiceInsight to view the visualization and when done, change the configuration back and restart the service.


## Custom serialization

The SagaAudit plugin serializes the saga data objects using a simple json serializer. This serializer should be fine for most use cases. It handles all primitive types (including `TimeSpan` and `DateTime`), their nullable variants as well as nested objects. However, if more sophisticated mechanism is required, the serialization method can be provided by the user:

snippet: SagaAuditNew_CustomSerialization
