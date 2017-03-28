Satellites are extension points to handle messages that arrive in different queues other than the endpoint's main queue. This extension point is useful when the robustness of a separate queue is needed without having to create and setup a new endpoint and configure any message mappings.

These extensions are available as `ISatellite` and `IAdvancedSatellite` interfaces.


### ISatellite

To use this extension, declare a class that implements the `ISatellite` interface. Specify values for the `InputAddress` and enable it as shown. The `InputAddress` is the queue that the satellite watches for messages.

snippet: SimpleSatelliteSetup

When messages are received in the queue specified as the `InputAddress`, the satellite's `Handle` method will be invoked.

snippet: SimpleSatelliteHandleMessages


### IAdvancedSatellite

To use this extension, declare a class that implements the `IAdvancedSatellite` interface. The `IAdvancedSatellite` offers additional customization. For example, as part of the receiving strategy, a custom error handling routine can be specified when there is a message processing error.

snippet: AdvancedSatelliteReceiverCustomization

In the above example, the `SatelliteImportFailuresHandler` is a custom error handling routine that implements the [IManageMessageFailures extension](/nservicebus/pipeline/customizing-error-handling.md).