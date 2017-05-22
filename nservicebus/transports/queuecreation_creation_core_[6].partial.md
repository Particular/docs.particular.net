
## Creation

Queues get created during [installation](/nservicebus/operations/installers.md) time only.

Transports need to implement a custom queue creator.

It is the responsibility of the queue creator to either sequentially or concurrently create the queues provided in the queue bindings for the specified identity.

Here is a sample of a queue creator

snippet: CustomQueueCreator

The custom queue creator needs to be registered.

snippet: RegisteringTheQueueCreator
