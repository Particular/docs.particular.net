Custom exception handling can be implemented using pipeline behaviors. To learn more about pipeline and behaviors refer to the documentation on [how to manipulate the pipeline with behaviors](/nservicebus/pipeline/manipulate-with-behaviors.md).

### Create a new behavior

Implement a new behavior, which extends the `ITransportReceiveContext` context interface. This context provides details about the message at the transport level. Calling `next()` in the pipeline will invoke the subsequent pipeline processing steps.

snippet: ErrorHandlingBehavior

#### Handling deserialization errors

To handle any deserialization errors, wrap the `next()` operation in a try-catch block and handle the `MessageDeserializationException` as shown:

snippet: DeserializationCustomization

WARNING: Throwing a `MessageDeserializationException` exception in the catch block will immediately forward the message to the error queue. A message that fails due to a `MessageDeserializationException` will not be retried. If the message must be consumed and removed from the queue, remove the `throw` from the catch block to indicate that the message has been successfully processed. 

#### Handling other errors

To handle other errors, wrap the `next()` operation in a try-catch block and handle the `Exception` as shown:

snippet: AllErrorsCustomization

WARNING: Throwing the exception in the catch block will forward the message to the error queue after all the configured retry attempts. If the message must be consumed and removed from the queue then remove the `throw` from the catch block to indicate that the message has been successfully processed.

### Registering the behavior

In the example below, the behavior `CustomErrorHandlingBehavior` is registered to be part of the message handling pipeline. This new behavior is placed at the very beginning of the pipeline. This placement allows inserting code right after a message has been received from the transport and right before the [recoverability](/nservicebus/recoverability/) policy is invoked.

snippet: RegisterCustomErrorHandlingBehavior
