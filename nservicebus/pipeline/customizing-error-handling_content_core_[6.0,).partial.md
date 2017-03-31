The `IManageMessageFailures` interface has been deprecated. Instead, the same functionality can be achieved by adding a new behavior which is invoked during the message processing pipeline. Read this article on how to [manipulate the pipeline with behaviors](/nservicebus/pipeline/manipulate-with-behaviors.md).


### Create a new behavior

Implement a new behavior, which extends the `ITransportReceiveContext` context. This context provides details about the message at the transport level. Calling `next()` in the pipeline will invoke the subsequent pipeline processing steps.

snippet: ErrorHandlingBehavior


#### Handling Deserialization Errors

To handle any deserialization errors, wrap the `next()` operation in a try-catch block and handle the `MessageDeserializationException` as shown:

snippet: DeserializationCustomization


#### Handling Other Errors

To handle any other errors, wrap the `next()` operation in a try-catch block and handle the `Exception` as shown:

snippet: AllErrorsCustomization

WARNING: Throwing the exception in the catch block will forward the message to the error queue. If that's not desired, remove the `throw` from the catch block to indicate that the message has been successfully processed.


#### Rolling Back

To rollback the receive operation, instead of either handling the message or to forward it to the error queue, invoke `AbortReceiveOperation` as shown below:

snippet: RollbackMessage


### Registering the Behavior

In the example below, the new behavior `CustomErrorHandlingBehavior` is registered to be part of the message handling pipeline. This new behavior is registered such that it will be invoked after NServiceBus has invoked the [recoverability mechanism](/nservicebus/recoverability/). This includes Second-Level Retries if they are enabled.

snippet: RegisterCustomErrorHandlingBehavior