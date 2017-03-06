---
title: Pipeline events
summary: How to subscribe to events raised by the pipeline
component: Core
reviewed:
tags:
- Pipeline
- Notifications
---

The pipeline exposed the following events.

## Receive Pipeline completed

Everytime a receive pipeline is completed a `ReceivePipelineCompleted` will be raised. This event will occur even if the message fails to be processed.

Use the following configuration code to subscribe to this event:

snippet: ReceivePipelineCompletedSubscriptionFromEndpointConfig

Subscribing from a [feature](/nservicebus/pipeline/features.md) is show below:

snippet: ReceivePipelineCompletedSubscriptionFromFeature

Note: A completed receive pipeline is not the same as the message being removed from the incoming queue. Infrastructure exceptions can still cause the message to be rolled back and reprocessed. See [receoverability](/nservicebus/recoverability/) for more details.
