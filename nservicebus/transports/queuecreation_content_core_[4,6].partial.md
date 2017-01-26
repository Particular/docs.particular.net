
## Declaration

Queues should be declared during the Setup phase of a [Feature](/nservicebus/pipeline/features.md).

A built-in example is the audit feature which needs the audit queue. During start-up NServiceBus ensures the declared queues are present and aborts the start-up procedure if they are not (with an exception of MSMQ remote queues which are optional).

snippet:queuebindings


## Creation

Queues get created during [installation](/nservicebus/operations/installers.md) time only.

Transports need to implement a custom queue creator.

In Version 6 and above it is the responsibility of the queue creator to either sequentially or concurrently create the queues provided in the queue bindings for the specified identity.

In Version 5 and below the queue creation process is always executed sequentially.

Here a sample of a queue creator

snippet:CustomQueueCreator

The custom queue creator needs to be registered.

snippet:RegisteringTheQueueCreator