---
title: Message Handling Pipeline
summary: Overview of the message handling pipeline 
tags:
- Pipeline
- NServiceBus
---

NServiceBus has always had the concept of a pipeline execution order that is executed when a message is received and also when a message is dispatched. NServiceBus v5 makes this pipeline a first level concept and exposes it to the end user.
This now allows the end users to take full control of the incoming and/or the outgoing built-in default functionality, it also allows to be easily extendable and/or completely replaced.

The pipeline is focused on the message lifecycle and is composed of `steps` whose role is to manage every single aspect of the message lifecycle during the incoming and outgoing phases.

The default pipeline is composed of several steps, each one dedicated to a specific aspect of the message lifecycle, the default steps are the following:

###Incoming Message Steps

* CreateChildContainer: Creates the child container;
* ExecuteUnitOfWork: Executes the Unit of Work;
* MutateIncomingTransportMessage: Executes IMutateIncomingTransportMessages;
* DeserializeMessages: Deserializes the physical message body into logical messages;
* ExecuteLogicalMessages: Starts the execution of each logical message;
* MutateIncomingMessages: Executes IMutateIncomingMessages;
* LoadHandlers: Executes all IHandleMessages<T>;
* InvokeHandlers: Calls the IHandleMessages<T>.Handle(T);
            
###Outgoing Message Steps

* EnforceBestPractices: Enforces messaging best practices;
* MutateOutgoingMessages: Executes IMutateOutgoingMessages;
* CreatePhysicalMessage: Converts a logical message into a physical message;
* SerializeMessage: Serializes the message to be sent out on the wire;
* MutateOutgoingTransportMessage: Executes IMutateOutgoingTransportMessages;
* DispatchMessageToTransport: Dispatches messages to the transport;

The following picture summarize the message lifecycle pipeline:

![Message lifecycle pipeline](001_pipeline.jpg)

###Anatomy of a Message Behaviors

A message behavior is a class that implements the `IBehavior<TContext>` interface:

    public class SampleBehavior : IBehavior<IncomingContext>
    {
    	public void Invoke(IncomingContext context, Action next)
    	{
    		next();
        }
    }

In the above sample the `SampleBehavior` class implements the `IBehavior<IncomingContext>` interface where the fact that the context type is `IncomingContext` determines that the above behavior will be applied to an incoming message.

In order to create a behavior that is applied to an outgoing message the `IBehavior<OutgoingContext>` can be implemented.

At runtime the message lifecycle pipeline will invoke the `Invoke` method of each registered behavior passing in as arguments the current message context and an action to invoke the next behavior in the pipeline.

**NOTE**: it is responsibility of each behavior to invoke the next behavior in the pipeline chain.

###Message Behaviors registration

Once a behavior is created the last step is to register it in the message handling pipeline:

    public class RegisterSampleBehavior : INeedInitialization
    {
        public void Init( Configure config )
        {
	       config.Pipeline.Register( "step unique id", typeof( SampleBehavior ), "Description of the sample step");
        }
    }

In the above sample the behavior is appended in the pipeline at the end and will be executed as the last behavior in the chain.

We can also replace existing behaviors using the `Replace` method and passing as the first argument the `id` of the step we want to replace:

    config.Pipeline.Replace( "id of the step to replace", typeof( NewBehaviorType ), "description" )
    
There is also the possibility to simply remove an existing behavior from the pipeline.

The last option we have is to create a custom behavior registration in order to control how the behavior is registered in the pipeline. In order to create a custom registration we need to create a class that inherits from the `RegisterStep` base class. The benefits of creating a custom step registration are:

* Express step dependencies in order to be sure that the pipeline at runtime is configured as we expect;
* Define the position of the step in the pipeline, as stated above registering a step will append it at the end of the pipeline, using a custom registration we can determine the position of the step in the pipeline. 

**NOTE**: Once a step is registered the behavior class lifecycle is managed by the NServiceBus Inversion of Control container thus behaviors can express dependencies, as public properties or constructor arguments, as any other component handled by the Inversion of Control container.
