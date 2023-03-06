In this sample, an external system sends a message to an SQS queue using the Amazon SQS SDK. In order for NServiceBus to be able to consume this message, a `NServiceBus.AmazonSQS.Headers` message attribute with an empty json string (`"{}"`) must be present. Other attributes are also included to demonstrate how to access those from handlers or behaviors in the pipeline.

NOTE: This is for NServiceBus endpoints running NServiceBus.AmazonSQS version 6.1 and above. Use the `MessageTypeFullName` message attribute for compatibility with earlier versions of the transport.

snippet: SendingANativeMessage