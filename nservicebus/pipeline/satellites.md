---
title: Migrating Satellites
summary: Extension point for handling messages
tags: 
related:
 - nservicebus/pipeline/customizing-v6
 - nservicebus/pipeline/features
redirects:
---

In NServiceBus Versions 5 and below, Satellites were extension points to handle messages that arrive in different queues other than the endpoint's main queue. This extension point came in handy when the robustness of a separate queue was needed without having to create and setup a brand new Endpoint and configure any needed message mappings.

These extensions were available via the `ISatellite` and `IAdvancedSatellite` interfaces.

Starting from Versions 6 and above, these interfaces are no longer available. However, the same functionality can be achieved using the more advanced `Feature` extension. 


## Migrating ISatellite and IAdvancedSatellite to Pipeline Features

In Versions 6 and above, to get the satellite processing behavior, create a new feature. For more details, please read this article [to create a new feature and enabling it](/nservicebus/pipeline/features.md#feature-api).


### Configuring the Satellite feature

Once the feature is created, it needs to be configured using a message pipeline behavior.

In the example below, the satellite input queue is called "targetqueue." 
       
snippet: SatelliteFeatureSetup

The satellite behavior class should inherit from `PipelineTerminator<ISatelliteProcessingContext>`. The `ISatelliteProcessingContext` provides the Terminate method where post-processing steps are run when a message arrives in the satellite queue. 

snippet: SatelliteBehavior

The `ISatelliteProcessingContext` parameter provides necessary details about the message such as the message body, headers, and other needed information about the message that arrived in the queue. 


## Injecting CriticalError

In Versions 5 and below, the `IAdvancedSatellite` offered additional customization when receiving messages in the satellite queue. For example, critical error processing. 

The endpoint's `CriticalError` action can be passed into the Satellite Behavior.

snippet: AdvancedSatelliteFeatureSetup

As part of the receiving strategy, the endpoint could be instructed to shutdown if a critical error occurs during the processing of a message inside the satellite behavior. 
 
snippet: AdvancedSatelliteBehavior

The NServiceBus message processing pipeline has been enhanced to handle more advanced scenarios. For more information, please read the article on [NServiceBus Pipeline In Versions 6 and above](/nservicebus/pipeline/customizing-v6.md).