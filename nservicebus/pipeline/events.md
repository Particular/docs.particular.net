---
title: Pipeline events
summary: Subscribe to events raised by the pipeline
component: Core
reviewed: 2020-11-26
versions: '[6,)'
related:
- nservicebus/recoverability
---

The pipeline raises the following notification events:


## Receive pipeline completed

Every time a receive pipeline is completed, a `ReceivePipelineCompleted` event will be raised. This event will not occur when message processing fails e.g. inside a handler.

Use the following configuration code to subscribe to the event:

snippet: ReceivePipelineCompletedSubscriptionFromEndpointConfig

Subscribing from a [feature](/nservicebus/pipeline/features.md) is shown below:

snippet: ReceivePipelineCompletedSubscriptionFromFeature

NOTE: A `ReceivePipelineCompleted` event being raised does not guarantee that the message has been removed from the incoming queue. Infrastructure exceptions can still cause the message to be rolled back and reprocessed.