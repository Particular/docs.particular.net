## Broker permissions

The transport uses the credentials from the connection string for all AMQP operations. RabbitMQ grants [configure, write, and read permissions](https://www.rabbitmq.com/docs/access-control) per virtual host, each as a regular expression that matches queue and exchange names. Configure permission covers declaring a queue or exchange. Write permission covers publishing to an exchange and being the destination of a binding. Read permission covers consuming from a queue and being the source of a binding.

When [installers](/nservicebus/operations/installers.md) are enabled, the transport also creates the endpoint's queues, exchanges, bindings, and delayed delivery infrastructure, so the credentials also need permission to create them. The tables below list the permissions an endpoint needs at runtime, after that infrastructure exists.

### Delayed delivery

The endpoint needs these permissions to send delayed messages, including delayed retries, regardless of the routing topology:

|Resource |Permissions
|:--- |:---
|The `nsb.v2.delay-level-00` to `nsb.v2.delay-level-27` exchanges |Write
|The `nsb.v2.delay-delivery` exchange |Read

The endpoint also needs write permission on the destination endpoint's exchange with the conventional topology, or its queue with the direct topology.

### Conventional routing topology

With the [conventional routing topology](routing-topology.md#conventional-routing-topology), an endpoint needs:

|Resource |Permissions |Used for
|:--- |:--- |:---
|Event exchanges, named `Namespace:TypeName`, for every event type the endpoint publishes or subscribes to, and for each base class and interface of that type |Configure, write, read |Publishing and subscribing
|The endpoint's own exchange |Write |Subscribing and unsubscribing
|Exchanges of the endpoints the endpoint sends to, including the error and audit queues |Write |Sending
|Queues the endpoint receives from |Read |Receiving

Unsubscribing removes a binding and needs only read permission on the event exchange.

> [!WARNING]
> When the endpoint publishes or subscribes to an event type for the first time in a process, the transport declares the exchanges for the type, its base classes, and its interfaces, and binds them together. RabbitMQ checks configure permission on every declaration, including a declaration of an exchange that already exists with the same arguments. The credentials therefore need configure permission on event exchanges even when every exchange already exists. Disabling installers does not remove this requirement.

### Direct routing topology

With the [direct routing topology](routing-topology.md#direct-routing-topology), an endpoint needs:

|Resource |Permissions |Used for
|:--- |:--- |:---
|The topic exchange, `amq.topic` by default |Write |Publishing
|The topic exchange |Read |Subscribing and unsubscribing
|A topic exchange other than `amq.topic` |Configure |Subscribing
|Queues the endpoint receives from |Read, write |Receiving (read), subscribing and unsubscribing (write)
|The default exchange, named `amq.default` for permission checks |Write |Sending

> [!WARNING]
> Subscribing requires configure permission on a custom topic exchange, even when it already exists and installers are disabled. Publishing does not require configure permission, nor does subscribing with the default `amq.topic` exchange.

### Installers and configure permission

Disabling [installers](/nservicebus/operations/installers.md) stops the transport from creating queues, endpoint exchanges, and the delayed delivery infrastructure. It does not stop the transport from declaring event exchanges when the endpoint publishes or subscribes, so the credentials still need configure permission on those exchanges.
