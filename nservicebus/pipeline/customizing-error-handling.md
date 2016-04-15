---
title: Migrating IManageMessageFailures
summary: Extension point for handling message failures.
related:
 - nservicebus/pipeline/customizing-v6
 - nservicebus/pipeline/features
---


## Versions 6 and Above

The `IManageMessageFailures` interface has been deprecated. Instead, the same functionality can be achieved by adding a new behavior which is invoked during the message processing pipeline. Read this article on how to [create new pipeline behaviors](/nservicebus/pipeline/customizing-v6.md).


### Create a new behavior.

Implement a new behavior, which extends the `ITransportReceiveContext` context. This context provides details about the message at the transport level. Calling `next()` in the pipeline will invoke the subsequent pipeline processing steps.

snippet: ErrorHandlingBehavior


#### Handling Deserialization Errors

To handle any deserialization errors, wrap the `next()` operation in a try-catch block and handle the `MessageDeserializationException` as shown:

snippet: DeserializationCustomization


#### Handling Other Errors

To handle any other errors, wrap the `next()` operation in a try-catch block and handle the `Exception` as shown:

snippet: AllErrorsCustomization

WARNING: Throwing the exception in the Catch block will forward the message to the error queue. If that's not desired, remove the `throw` from the catch block to indicate that the message has been successfully processed.

#### Rolling Back 

If it's required to rollback the receive operation instead of either handling the message or to forward it to the error queue, invoke `AbortReceiveOperation` as shown below: 

snippet: RollbackMessage


### Registering the Behavior

In the example below, the behavior `CustomErrorHandlingBehavior` is being registered in the message handling pipeline. This behavior will be invoked after NServiceBus has invoked the first-level retries. 

snippet: RegisterCustomErrorHandlingBehavior
            
## Versions 5 and Below

In NServiceBus Versions 5 and below, `IManageMessageFailures` are extension points to customizing actions when messages continue to fail after the [First Level Retries](/nservicebus/errors/automatic-retries.md) have been attempted. 


snippet: CustomFaultManager

This extension needs to be registered in the container so that it can be invoked when the failures occur. Registration of this component can be done using the `INeedInitialization` interface. Read this article for more details on [how to register custom components](/nservicebus/containers/child-containers.md) in the container.

snippet: RegisterFaultManager

