NOTE: This is advanced documentation and only of significant relevance when developing custom behaviors or satellites. To make a long story short: As from version 7, transactional operations are enlisted in the receive scope, if present (only in `SendAtomicWithReceive` mode). And all transactional operations performed in the pipeline must be wrapped by a suppress scope. Read on to learn why.

In contrast to previous versions, NServiceBus version 6 and above provides the transport full control on how it implements the transactional support. A transaction scope is no longer mandatory in order to orchestrate dispatching of messages with complete/rollback behavior. The Azure Service Bus transport now no longer enlists in the same transaction scope that is used to access the database in handler implementations. 

In fact it no longer uses a transaction scope at all except when configured using the `SendAtomicWithReceive` transaction mode. In this case it opens a new receive scope and ensures this new scope does not automatically promote the handler scope by wrapping the handler scope in a suppress scope. 

The new architecture for `SendAtomicWithReceive` is schematically represented in the diagram:

![Transactions v7](transactions-v7.png)

As illustrated in the diagram, the NServiceBus pipeline consists of 3 major parts:
* The receive section, which is invoked by the transport
* The handler invocation section responsible for invoking the implementations of `IHandleMessages`.
* And finally the dispatch section, responsible for sending out messages through the transport.

Note that the pipeline is not straight, there is a [fork in the pipeline](/nservicebus/pipeline/steps-stages-connectors.md) that is separating the user code invocation path from the dispatching path. Dispatching only happens after the handler invocation section has returned, and all implementations of `IHandleMessages` have been executed.

This is important as it allows the transport to flow it's transaction scope from the receive section to the dispatch section. While at the same time it can prevent that scope from promoting the handler scope by putting a suppress scope around that section of the pipeline.

Flowing the receive scope into the dispatch section is a requirement to allow the Azure Service Bus transport to take advantage of, a little-known capability of the Azure Service Bus SDK, [the via entity path / transfer queue](https://github.com/Azure-Samples/azure-servicebus-messaging-samples/tree/master/AtomicTransactions). 

Using this feature, send operations to different Azure Service Bus entities can be executed via a single entity, usually the receive queue and be completed in a single operation together with the acknowledgment that the receive operation has completed.  

Schematically it works like this:

![Send Via](send-via.png)

Combining these capabilities, allows Azure Service Bus Transport to support `SendAtomicWithReceive`, `ReceiveOnly` and `None` transaction mode levels.

NOTE: When developing a satellite or behavior it needs to be taken into account that `SendAtomicWithReceive` will wrap the satellite in a transaction scope which cannot be used by other transactional resources in the satellite, therefor a suppress scope must be added around the invocation of any other transactional resources used inside the satellite implementation.

include: send-atomic-with-receive-note