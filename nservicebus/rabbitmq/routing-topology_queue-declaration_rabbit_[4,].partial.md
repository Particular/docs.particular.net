### Taking control of queue declaration

Starting with Version 4.2, it is possible to take control of queue declaration by implementing `IDeclareQueues` in addition to `IRoutingTopology` in the routing topology class.

When the routing topology implements `IDeclareQueues`, the transport will not declare queues, and it will call the `DeclareAndInitialize` method of the routing topology instead of calling the `Initialize` method. The routing topology is then responsible for creating the queue and any exchanges, bindings and extra queues which are required for that queue to operate. The `Initialize` method of the routing topology will not be called.

`IRoutingTopology` and `IDeclareQueues` will be combined into a single interface in version 5.0.
