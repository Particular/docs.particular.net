
## Implementing a satellite

The satellite infrastructure will allow users to handle messages as they become available on the input queue. To create a satellite place the following code in the `Setup` method of a [feature](/nservicebus/pipeline/features.md#feature-api):

snippet: SimpleSatelliteFeature

The call to `AddSatelliteReceiver` registers the action to take when a message is received. In the above example, the satellite will watch a queue named `targetQueue`. 

The `PushContext` parameter provides necessary details about the message such as the message body, headers, and other needed information about the message that arrived in the queue. If migrating from a Satellite extension from Versions 5 and below, the implementation steps that were in the `Handle` method would go into the func provide to the `AddSatelliteReceiver` method.


### Managing errors

Problems processing the message needs to be handled by the satellite implementation. Options are immediate retry, delayed retry or move the message to the error queue.

To request a [immediate retry](/nservicebus/recoverability/#immediate-retries) use:

snippet: SatelliteRecoverability-ImmediateRetry

To request a [delayed retry](/nservicebus/recoverability/#delayed-retries) use:

snippet: SatelliteRecoverability-DelayedRetry

To request the message to be moved to the error queue use the following:

snippet: SatelliteRecoverability-ErrorQueue


### Injecting CriticalError

In Versions 5 and below, the `IAdvancedSatellite` offered additional customization when receiving messages in the satellite queue by implementing the `GetReceiverCustomization()` method. For example, critical error processing or error handling customization, etc. In Versions 6 and above `CriticalError` can be resolved via the builder and be used to instruct the endpoint to shutdown if a critical error occurs during the processing of a message.

snippet: AdvancedSatelliteFeatureSetup