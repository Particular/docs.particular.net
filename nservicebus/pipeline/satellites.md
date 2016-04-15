---
title: Migrating Satellites
summary: Extension point for handling messages.
reviewed: 2016-04-11
related:
 - nservicebus/pipeline/customizing-v6
 - nservicebus/pipeline/features
---


## Versions 6 and above

In Versions 6 and above, both the `ISatellite` and the `IAdvancedSatellite` interfaces are deprecated. The same functionality is available by using the message processing pipeline `Feature`.

To get the same behavior as the `ISatellite` or `IAdvancedSatellite` extension, create a new pipeline feature and enable it. See also [Feature API documentation](/nservicebus/pipeline/features.md#feature-api).

snippet: SimpleSatelliteFeature

The call to `AddSatellitePipeline` in the `Setup` method creates a new message processing pipeline for the satellite. In this call, the queue that needs to be watched is configured. In the above example, the satellite will watch a queue named `targetqueue`.

The action of what should occur when the messages arrive at the `targetqueue` is specified as a new `Behavior`. See also [Create pipeline behaviors](/nservicebus/pipeline/customizing-v6.md).

The satellite behavior class should inherit from **`PipelineTerminator<ISatelliteProcessingContext>`**. The `ISatelliteProcessingContext` provides the Terminate method where post-processing steps are run when a message arrives in the satellite queue.

snippet: SatelliteBehavior

The `ISatelliteProcessingContext` parameter provides necessary details about the message such as the message body, headers, and other needed information about the message that arrived in the queue. If migrating from a Satellite extension from Versions 5 and below, the implementation steps that were in the `Handle` method would go into the `Terminate` method.


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


## Injecting CriticalError

In Versions 6 and above, the message processing pipeline can handle more advanced scenarios. Some examples are outlined in [NServiceBus Pipeline Samples](/samples/pipeline/) for further details.

In Versions 5 and below, the `IAdvancedSatellite` offered additional customization when receiving messages in the satellite queue by implementing the `GetReceiverCustomization()` method. For example, critical error processing or error handling customization, etc. However, In Versions 6 and above, the same can be achieved by the message pipeline as explained in the previous section. Additional attributes like the endpoint's `CriticalError` can be passed in as part of the pipeline's registration step. The endpoint can also be instructed to shutdown if a critical error occurs during the processing of a message inside the satellite behavior.

snippet: AdvancedSatelliteFeatureSetup

snippet: AdvancedSatelliteBehavior