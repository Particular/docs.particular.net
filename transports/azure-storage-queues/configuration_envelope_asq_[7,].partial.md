## Custom envelope unwrapper

Azure Storage Queues lacks native header support. NServiceBus solves this by wrapping headers and message body in a custom envelope structure. This envelope is serialized using the configured [serializer](/nservicebus/serialization) for the endpoint before being sent.

Creating this envelope can cause uneeded complexity should headers not be needed for example in native integration scenarios. For this reason Version 7.1 and above support configuring a custom envelop unwrapper. 

The snippet below shows custom unwrapping logic that enables both NServiceBus formatted and plain JSON messages to be consumed.

snippet: CustomEnvelopeUnwrapper 

NOTE: This feature is currently NOT compatible with ServiceControl. A [ServiceControl transport adapter](/servicecontrol/transport-adapter/) is required in order to leverage both.
