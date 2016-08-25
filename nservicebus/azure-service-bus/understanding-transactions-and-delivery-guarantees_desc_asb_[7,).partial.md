In NServiceBus version 6 and above transports fully control the transactional support. A transaction scope is no longer mandatory in order to orchestrate dispatching of messages with complete/rollback behavior. The Azure Service Bus transport only uses a transaction scope to implement the `SendAtomicWithReceive` transaction mode. Other modes work without using transaction scope. 

The new architecture for `SendAtomicWithReceive` is schematically represented in the diagram:

![Transactions v7](transactions-v7.png)

Note the [fork in the pipeline](/nservicebus/pipeline/steps-stages-connectors.md) which is separating the user code invocation path from the dispatching path. By putting a suppress scope around this section of the pipeline, the transport can prevent any other transactional resource from enlisting in the scope.

Furthermore, the Azure Service Bus transport also takes advantage of a little-known capability of the Azure Service Bus SDK, [the via entity path / transfer queue](https://github.com/Azure-Samples/azure-servicebus-messaging-samples/tree/master/AtomicTransactions). Using this feature, send operations to different Azure Service Bus entities can be executed via a single entity, usually the receive queue.  

Schematically it works like this:

![Send Via](send-via.png)

Combining these capabilities, allows Azure Service Bus Transport to support `SendAtomicWithReceive`, `ReceiveOnly` and `None` transaction mode levels.

NOTE: When developing a satellite it needs to be taken into account that `SendAtomicWithReceive` will wrap the satellite in a transaction scope which cannot be reused by other transactional resources, there a suppress scope should be added around the invocation of any other transactional resources.

include: send-atomic-with-receive-note