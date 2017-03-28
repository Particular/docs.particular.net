---
title: Pipeline events
summary: Subscribe to events raised by the pipeline
component: Core
reviewed: 2017-03-28
versions: '[6,)'
tags:
- Pipeline
- Notifications
related: 
- nservicebus/recoverability
---

The pipeline raises the following notification events:


## Receive pipeline completed

Every time a receive pipeline is completed, a `ReceivePipelineCompleted` event will be raised. This event will occur even if the message fails to be processed.

Use the following configuration code to subscribe to this event:

snippet: ReceivePipelineCompletedSubscriptionFromEndpointConfig

Subscribing from a [feature](/nservicebus/pipeline/features.md) is shown below:

snippet: ReceivePipelineCompletedSubscriptionFromFeature

NOTE: A `ReceivePipelineCompleted` event being raised does not guarantee that the message has been removed from the incoming queue. Infrastructure exceptions can still cause the message to be rolled back and reprocessed.