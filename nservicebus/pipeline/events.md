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

The pipeline exposed the following events.


## Receive Pipeline completed

Every time a receive pipeline is completed a `ReceivePipelineCompleted` event will be raised. This event will occur even if the message fails to be processed.

Use the following configuration code to subscribe to this event:

snippet: ReceivePipelineCompletedSubscriptionFromEndpointConfig

Subscribing from a [feature](/nservicebus/pipeline/features.md) is show below:

snippet: ReceivePipelineCompletedSubscriptionFromFeature

NOTE: A completed receive pipeline is not the same as the message being removed from the incoming queue. Infrastructure exceptions can still cause the message to be rolled back and reprocessed.