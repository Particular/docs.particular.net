### Custom envelope unwrapper

Azure Storage Queues lacks native header support. NServiceBus solves this by wrapping headers and message body in a custom envelope structure. This envelope is serialized using the configured [serializer](/nservicebus/serialization) for the endpoint before being sent.

Creating this envelope can cause unnecessary complexity if headers are not needed, as is the case in native integration scenarios. For this reason, NServiceBus.Transport.AzureStorageQueues 9.0 and above support configuring a custom envelope unwrapper. 

> [!WARNING]
> In this scenario, NServiceBus may place messages in your queue in addition to the native messages that are expected, for example if a message results in a [delayed retry](delayed-delivery.md). Any custom envelope unwrapper must verify if the incoming message is capable of being deserialized as a native message, and the resulting body must be serialized according to the configured endpoint serializer.

The snippet below shows custom unwrapping logic that enables both NServiceBus formatted and plain serialized messages to be consumed.

snippet: CustomEnvelopeUnwrapper

> [!NOTE]
> This feature is currently NOT compatible with ServiceControl. A [ServiceControl transport adapter](/servicecontrol/transport-adapter.md) is required to leverage both.
