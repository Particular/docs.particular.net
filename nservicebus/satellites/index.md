---
title: Satellites
summary: Extension point for raw processing of messages.
reviewed: 2016-04-16
related:
 - nservicebus/pipeline/manipulate-with-behaviors
 - nservicebus/pipeline/features
redirects:
 - nservicebus/pipeline/satellites
---

Satellites are light weight message processors that runs in the same process as its owning endpoint. While are mostly used by NServiceBus to implement infastructure features like the TimeoutManager and the Gateway they can be used in scenarios where messages from additional queues other than the main input queue needs to be processed. This is useful when the robustness of a separate queue is needed without having to create and setup a new endpoint and configure any message mappings.
 
## Implementing a satellite

The satellite infastructure will allow users to handle messages as they become available on the input queue. To create a satellite place the following code in the `Setup` method of a [feature](/nservicebus/pipeline/features.md#feature-api):

snippet: SimpleSatelliteFeature

The call to `AddSatelliteReceiver` registers the action to take when a message is recived. In the above example, the satellite will watch a queue named `targetQueue`. 

The `PushContext` parameter provides necessary details about the message such as the message body, headers, and other needed information about the message that arrived in the queue. If migrating from a Satellite extension from Versions 5 and below, the implementation steps that were in the `Handle` method would go into the func provide to the `AddSatelliteReceiver` method.


### Managing errors

Problems processing the message needs to be handled by the satellite implementor. Options are immediate retry, delayed retry or move the message to the error queue.

To request a immediate retry use:

snippet: SatelliteRecoverability-ImmediateRetry

To request a delayed retry use:  

snippet: SatelliteRecoverability-DelayedRetry

To request the message to be moved to the error queue use the following:

snippet: SatelliteRecoverability-ErrorQueue


### Injecting CriticalError

In Versions 5 and below, the `IAdvancedSatellite` offered additional customization when receiving messages in the satellite queue by implementing the `GetReceiverCustomization()` method. For example, critical error processing or error handling customization, etc. In Versions 6 and above `CriticalError` can be resolved via the builder and be used to instruct the endpoint to shutdown if a critical error occurs during the processing of a message.

snippet: AdvancedSatelliteFeatureSetup

## Versions 5 and below

In NServiceBus Versions 5 and below, Satellites are extension points to handle messages that arrive in different queues other than the endpoint's main queue. This extension point is useful when the robustness of a separate queue is needed without having to create and setup a new endpoint and configure any message mappings.

These extensions are available as `ISatellite` and `IAdvancedSatellite` interfaces.


### ISatellite

To use this extension, declare a class that implements the `ISatellite` interface. Specify values for the `InputAddress` and enable it as shown. The `InputAddress` is the queue that the satellite watches for messages.

snippet:SimpleSatelliteSetup

When messages are received in the queue specified as the `InputAddress`, the satellite's `Handle` method will be invoked.

snippet:SimpleSatelliteHandleMessages


### IAdvancedSatellite

To use this extension, declare a class that implements the `IAdvancedSatellite` interface. The `IAdvancedSatellite` offers additional customization. For example, as part of the receiving strategy, a custom error handling routine can be specified when there is a message processing error.

snippet: AdvancedSatelliteReceiverCustomization

In the above example, the `SatelliteImportFailuresHandler` is a custom error handling routine that implements the [IManageMessageFailures extension](/nservicebus/pipeline/customizing-error-handling.md).