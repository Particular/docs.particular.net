Custom exception handling can be implemented using pipeline behaviors. To learn more about pipeline and behaviors refer to the documentation on [how to manipulate the pipeline with behaviors](/nservicebus/pipeline/manipulate-with-behaviors.md).

### Create a new behavior

Implement a new behavior, which extends the `ITransportReceiveContext` context interface. This context provides details about the message at the transport level. Calling `next()` in the pipeline will invoke the subsequent pipeline processing steps.

snippet: ErrorHandlingBehavior

#### Handling Deserialization Errors

To handle any deserialization errors, wrap the `next()` operation in a try-catch block and handle the `MessageDeserializationException` as shown:

snippet: DeserializationCustomization

WARNING: Throwing the exception in the catch block will immediately forward the message to the error queue. If a message fails due to a `MessageDeserializationException` the message won't be retried. If that's not desired, remove the `throw` from the catch block to indicate that the message has been successfully processed.

#### Handling Other Errors

To handle any other errors, wrap the `next()` operation in a try-catch block and handle the `Exception` as shown:

snippet: AllErrorsCustomization

WARNING: Throwing the exception in the catch block will forward the message to the error queue after all the configured retry attempts. If that's not desired, remove the `throw` from the catch block to indicate that the message has been successfully processed.

### Registering the Behavior

In the example below, the new behavior `CustomErrorHandlingBehavior` is registered to be part of the message handling pipeline. This new behavior is placed at the very beginning of the pipeline. Such placement allows to insert code right after a message has been received from the transport and right before the [recoverability](/nservicebus/recoverability/) policy is invoked.

snippet: RegisterCustomErrorHandlingBehavior
