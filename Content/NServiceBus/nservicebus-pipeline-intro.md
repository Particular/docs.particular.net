title: Message Handling Pipeline
summary: Overview of the message handling pipeline 
tags:
- Pipeline
- NServiceBus
---

NServiceBus v5 introduces a new message handling pipeline whose role is to allow end users to easily extend and replace built-in behaviors.

The pipeline is focused on the message lifecycle and is composed of `behaviors` whose role is to manage every single aspect of the message lifecycle during the incoming phase and during the outgoing phase.

The default pipeline is composed of several behaviors, each one dedicated to a specific aspect of the message lifecycle, the default behaviors are the following:

###Incoming Message Behaviors

* CreateChildContainer behavior: Creates the child container;
* ExecuteUnitOfWork behavior: Executes the UoW;* MutateIncomingTransportMessage behavior: Executes IMutateIncomingTransportMessages;* DeserializeMessages behavior: Deserializes the physical message body into logical messages;* ExecuteLogicalMessages behavior: Starts the execution of each logical message;* MutateIncomingMessages behavior: Executes IMutateIncomingMessages;* LoadHandlers behavior: Executes all IHandleMessages<T>;* InvokeHandlers behavior: Calls the IHandleMessages<T>.Handle(T);
            
###Outgoing Message Behaviors

* EnforceBestPractices behavior: Enforces messaging best practices;* MutateOutgoingMessages behavior: Executes IMutateOutgoingMessages;* CreatePhysicalMessage behavior: Converts a logical message into a physical message;* SerializeMessage behavior: Serializes the message to be sent out on the wire;* MutateOutgoingTransportMessage behavior: Executes IMutateOutgoingTransportMessages;* DispatchMessageToTransport behavior: Dispatches messages to the transport;

The following picture summarize the message lifecycle pipeline:

![Message lifecycle pipeline](001_pipeline.jpg)

###Anatomy of a Message Behaviors

A message behavior is a class that implements the `IBehavior<TContext>` interface:

    public class SampleBehavior : IBehavior<IncomingContext>
    {
    	public void Invoke(IncomingContext context, Action next)
    	{
    		next();        }
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
	       config.Pipeline.Register( "behavior unique id", typeof( SampleBehavior ), "Description of the sample behavior");
        }
    }

In the above sample the behavior is appended in the pipeline at the end and will be executed as the last behavior in the incoming chain.


I vior possono essere rimpiazzati, aggiunti o rimossi, un bejavior implementa IBehavior<TContext> dove TContext determina se il bejavior è outgoing o incoming

di default l'aggiunta di un behavior lo piazza in coda a quelli esistenti, per poter determinare dove inserirlo è necessario creare un RegisterBehavior custom che possa definire le dipendenze, e quindi chi deve venire prima chi dopo o entrambi

al fine di iniettare roba nella pipeline behavior: INeedInitialization