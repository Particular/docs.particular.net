---
title: Satellites
summary: Extension point for raw processing of messages.
reviewed: 2025-04-02
component: Core
versions: '[5,]'
related:
 - nservicebus/pipeline/manipulate-with-behaviors
 - nservicebus/pipeline/features
 - nservicebus/recoverability
redirects:
 - nservicebus/pipeline/satellites
---

Satellites are lightweight message processors that run in the same process as their owning endpoint. While they are mostly used by NServiceBus to implement infrastructure features like the Gateway, they can be used in scenarios where messages from additional queues other than the main input queue need to be processed. This is useful when the robustness of a separate queue is needed without having to create and setup a new endpoint and configure any message mappings.


## Implementing a satellite

The satellite infrastructure allows the handling of messages as they become available on the input queue. To create a satellite, place the following code in the `Setup` method of a [feature](/nservicebus/pipeline/features.md#feature-api):

snippet: SimpleSatelliteFeature

The call to `AddSatelliteReceiver` registers the action to take when a message is received. In the above example, the satellite will watch a queue named `targetQueue`.

The `MessageContext` parameter provides details about the incoming message such as the body and headers. When migrating from a satellite extension from NServiceBus version 5 and below, the implementation steps that are in the `Handle` method go into the func provided to the `AddSatelliteReceiver` method.


### Managing errors

Problems processing the message need to be handled by the satellite implementation. The available options are: immediate retry, delayed retry, or moving the message to the error queue.

To request an [immediate retry](/nservicebus/recoverability/#immediate-retries):

snippet: SatelliteRecoverability-ImmediateRetry

To request a [delayed retry](/nservicebus/recoverability/#delayed-retries):

snippet: SatelliteRecoverability-DelayedRetry

To request the the message be moved to the error queue:

snippet: SatelliteRecoverability-ErrorQueue


### Injecting CriticalError

In NServiceBus version 5 and below, the `IAdvancedSatellite` offers additional customization when receiving messages in the satellite queue by implementing the `GetReceiverCustomization()` method. For example, critical error processing or error handling customization, etc. In NServiceBus version 6 and above, `CriticalError` can be resolved via the builder and be used to instruct the endpoint to shutdown if a critical error occurs during the processing of a message.

snippet: AdvancedSatelliteFeatureSetup
