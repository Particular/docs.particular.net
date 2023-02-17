In this sample, an external system sends a message to an SQS queue using the Amazon SQS SDK. In order for NServiceBus to be able to consume this message, a `MessageTypeFullName` message attribute must be present. Other attributes are also included to demonstrate how to access those from handlers or behaviors in the pipeline.

snippet: SendingANativeMessage
