---
title: Customizing the Pipeline
summary: Customizing the message handling pipeline
tags:
- Pipeline
redirects:
- nservicebus/pipeline/customizing
---

NServiceBus has always had the concept of a pipeline execution order that is executed when a message is received and also when a message is dispatched. NServiceBus Version 5 makes this pipeline a first level concept and exposes it for extensibility. This now allows end users to take full control of the incoming and outgoing functionality.

In NServiceBus Version 5, there are two explicit pipelines: one for the outgoing messages and one for the incoming messages. Each pipeline is composed of "Steps". The steps have built-in behavior and this behavior can now be easily replaced. A completely new step containing new behavior can also be added to the pipeline.

A step is an identifiable value in the pipeline. The steps are used to programmatically define order of execution in the pipeline. Each step is a placeholder for a behavior which is the actual code. Each behavior is responsible for invoking the next item in the pipeline.

The steps in the processing pipeline are dynamic in nature. They are added or removed based on what features are enabled. For example, if an endpoint has Sagas then the Saga feature will be enabled by default, which in turn adds extra steps to the incoming pipeline to facilitate the handling of sagas.


## Some of the commonly used steps

Because steps can be added into the pipeline and or replaced based on the features that are enabled or disabled by each endpoint, listed below are only some of the steps for the basic incoming and outgoing pipeline.


### Incoming Message Pipeline

- `CreateChildContainer`: NServiceBus heavily relies on IoC to work properly and requires every message to be handled in its own context. Every message that arrives to an Endpoint will first create a new child container to generate a new dependency resolution scope;
* `ExecuteUnitOfWork`: This behavior is responsible for the creation of the Unit of Work, that wraps every message execution. The role of the Unit of Work is to guarantee the message is executed in a transactional fachion;
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

Although the execution order of the built-in pipeline cannot be changed, it is possible to change the built-in behavior of these steps and/or new steps can be added to the pipeline.


## How to code behaviors?

A message behavior is a class that implements the `IBehavior<TContext>` interface:

snippet:SamplePipelineBehavior

In the above code snippet the `SampleBehavior` class implements the `IBehavior<IncomingContext>` interface. This tells the framework to execute this behavior against the incoming pipeline. If you want to create a behavior that needs to be applied to the outgoing pipeline, implement the `IBehavior<OutgoingContext>` instead.

Sometimes a parent behavior might need to pass in some information relating to a child behavior and vice versa. The context facilitates this passing of data between behaviors in the pipeline steps. The context is a dictionary. You can add information to this dictionary in a parent behavior and retrieve this value from a child behavior and vice versa.

An important fact that the above sample outlines is that each behavior is responsible to call the `next` behavior in the chain and it is also interesting to note that a behavior can perform some actions before calling the next behavior in the chain and some other action after the chain has been executed.


## How does the pipeline execute its steps?

The pipeline is implemented using the [Russian Doll](https://en.wikipedia.org/wiki/Matryoshka_doll) Model. Russian dolls are a series of progressively smaller dolls nested within each other. Similarly, the pipeline model is a series of progressively nested steps within each other.

At runtime, the pipeline will call the `Invoke` method of each registered behavior passing in as arguments the current message context and an action to invoke the next behavior in the pipeline. It is responsibility of each behavior to invoke the next behavior in the pipeline chain.


## How to register a behavior?

Once a behavior is created we need to specify, which step is going to be implementing this new behavior in the pipeline. For example, is a new step going to contain this behavior or is it going to replace existing behavior of a built-in step.


### How to create a new step?

To do this:

1. Create a class that implements `RegisterStep`.
2. Register the step itself in the pipeline.

snippet:NewStepInPipeline


### How to replace behavior of a built-in step?

We can also replace existing behaviors using the `Replace` method and passing as the first argument the `id` of the step we want to replace. For example:

snippet:ReplacePipelineStep


## Exception Handling


### MessageDeserializationException (Version 5.1 and up)

As of Version 5.1 if a message fails to deserialize a `MessageDeserializationException` will be thrown.


#### When to throw

The implementation of `DeserializeLogicalMessagesBehavior` handles deserialization and can throw `MessageDeserializationException`. So any behavior that replaces `DeserializeLogicalMessagesBehavior` should duplicate this functionality.


#### When to handle

The implementation of `UnitOfWorkBehavior` handles aggregating multiple exceptions that can occur in a unit of work. However `MessageDeserializationException` is re-thrown (not aggregated). So any behavior that replaces  `UnitOfWorkBehavior` should duplicate this functionality.