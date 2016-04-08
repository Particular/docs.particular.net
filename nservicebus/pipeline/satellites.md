---
title: Migrating Satellites
summary: Extension point for handling messages 
related:
 - nservicebus/pipeline/customizing-v6
 - nservicebus/pipeline/features
redirects:
---

In NServiceBus Versions 5 and below, Satellites are extension points to handle messages that arrive in different queues other than the endpoint's main queue. This extension point was useful when the robustness of a separate queue was needed without having to create and setup a new Endpoint and configure any message mappings.

In Versions this extension is available via the `ISatellite` and `IAdvancedSatellite` interfaces.

In Versions 6 and above, these interfaces are not available. However, the same functionality is avaliable via the `Feature` API.


## Migrating ISatellite and IAdvancedSatellite to Pipeline Features

In Versions 6 and above, to get the satellite processing behavior, create a new feature. See also [Creating a new feature and enabling it](/nservicebus/pipeline/features.md#feature-api).


### Configuring the Satellite feature

Once the feature is created, it needs to be configured using a message pipeline behavior.

In the example below, the satellite input queue is called "targetqueue".

snippet: SatelliteFeatureSetup

The satellite behavior class should inherit from `PipelineTerminator<ISatelliteProcessingContext>`. The `ISatelliteProcessingContext` provides the Terminate method where post-processing steps are run when a message arrives in the satellite queue.

snippet: SatelliteBehavior

The `ISatelliteProcessingContext` parameter provides necessary details about the message such as the message body, headers, and other needed information about the message that arrived in the queue.


## Injecting CriticalError

In Versions 5 and below, the `IAdvancedSatellite` offered additional customization when receiving messages in the satellite queue. For example, critical error processing.

The endpoint's `CriticalError` action can be passed into the Satellite Behavior.

snippet: AdvancedSatelliteFeatureSetup

As part of the receiving strategy, the endpoint can be instructed to shutdown if a critical error occurs during the processing of a message inside the satellite behavior.
 
snippet: AdvancedSatelliteBehavior

The NServiceBus message processing pipeline can handle more advanced scenarios. See also [NServiceBus Pipeline In Versions 6 and above](/nservicebus/pipeline/customizing-v6.md).