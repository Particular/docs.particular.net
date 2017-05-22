
## Creation

Queues get created during [installation](/nservicebus/operations/installers.md) time only.

Transports need to implement a custom queue creator.

The queue creation process is always executed sequentially.

Here is a sample of a queue creator

snippet: CustomQueueCreator

The custom queue creator needs to be registered.

snippet: RegisteringTheQueueCreator
