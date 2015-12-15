---
title: Customizing the Pipeline in NSB v5
summary: Customizing the message handling pipeline in NServiceBus version 5.
tags:
- Pipeline
related:
- nservicebus/pipeline/customizing
---

In NServiceBus version 5, there are two explicit pipelines: one for the outgoing messages and one for the incoming messages. You can extend the pipeline with a custom behavior implementing `IBehavior<IncomingContext>` or `IBehavior<OutgoingContext>`.

snippet:SamplePipelineBehavior

In the above code snippet the `SampleBehavior` class implements the `IBehavior<IncomingContext>` interface. This tells the framework to execute this behavior against the incoming pipeline. At runtime, the pipeline will call the `Invoke` method of each registered behavior passing in as arguments the current message context and an action to invoke the next behavior in the pipeline.

Warning: Each behavior is responsible to call the next step in the pipeline chain by invoking `next`.


## Add a new step

To add your custom behavior to the pipeline, you need to define a step for it:

snippet:NewPipelineStep

Then register the new step in the pipeline settings:

snippet:AddPipelineStep


## Replace an existing step

To replace the implementation of an existing step, you can replace it with your custom behavior:

snippet:ReplacePipelineStep


## Some of the commonly used steps

To help you adding your custom steps at the right place within the pipeline, the essential built-in steps are listed in the `WellKnownStep` class. The following list describes some of the listed steps:


### Incoming Message Pipeline

- `CreateChildContainer`: NServiceBus heavily relies on IoC to work properly and requires every message to be handled in its own context. Every message that arrives to an Endpoint will first create a new child container to generate a new dependency resolution scope;
* `ExecuteUnitOfWork`: This behavior is responsible for the creation of the Unit of Work, that wraps every message execution. The role of the Unit of Work is to guarantee the message is executed in a transactional fashion;
* `MutateIncomingTransportMessage`: NServiceBus has the concept of [message mutators](/nservicebus/pipeline/message-mutators.md). This behavior is responsible for executing each registered `TransportMessage` mutator;
* `DeserializeMessages`: The DeserializeMessages behavior will deserialize the incoming message from its raw form, the `TransportMessage`, to a well known `class` or `interface` instance using the configured serializer;
* `ExecuteLogicalMessages`: This behavior is responsible for creating a dedicated context for each incoming message and to determine if there is any message other than built-in control messages that must be executed;
* `MutateIncomingMessages`: Once a TransportMessage has been deserialized it is passed through a new set of message mutators. This behavior is responsible for executing each registered message mutator;
* `LoadHandlers`: The LoadHandlers behavior will load all the handlers registered for the incoming messages and coordinate the execution logic of all the loaded handlers;
* `InvokeHandlers`: This behavior is responsible for physically invoking each message handler;


### Outgoing Message Pipeline

- `EnforceBestPractices`: This behavior is responsible for ensuring that best practices are respected. One example of this is that the user is not trying to `send` an event or to `publish` a command;
* `MutateOutgoingMessages`: Each message (like incoming messages) is passed to a set of message mutators that have the opportunity to manage and mutate the outgoing message;
* `CreatePhysicalMessage`: This behavior transforms the logical messages that need to be sent to the corresponding `TransportMessage`;
* `SerializeMessage`: This behavior takes care of using the configured serialization engine to serialize the outgoing message;
* `MutateOutgoingTransportMessage`: Before the `TransportMessage` is dispatched to the physical transport all the outgoing message mutators are invoked;
* `DispatchMessageToTransport`: The last behavior is to dispatch the `TransportMessage` to the underlying transport;


## Sharing data between behaviors

Sometimes a parent behavior might need to pass some information to a child behavior and vice versa. The `context` parameter of a behavior's Invoke method facilitates  passing data between behaviors. The context is very similar to a shared dictionary which allows adding and retrieving information from different behaviors.

snippet:SharingBehaviorData


include: customizing-exception-handling
