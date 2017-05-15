---
title: ServiceControl Transport Adapter
summary: placeholder
component: SCTransportAdapter
---

The ServiceControl Transport Adapter decouples ServiceControl from specifics of business endpoints transport. 


## Usage scenarios

The ServiceControl Transport Adapter can be useful in three scenarios:


### Mixed transports

Some endpoints in the system use different transport than the rest of the system but should report to the same instance of ServiceControl. This can happen for various reasons like integration with external parties or requirement to use a specific transport feature in subset of endpoints (e.g. MSMQ store-and-forward for occasionally connected endpoints).

In this case a Transport Adapter can be used for the subset of endpoints that use different transport. This allows to manage the whole system from a single instance of ServicePulse.

Following code shows the configuration of the Transport Adapter in the mixed transport scenario:

snippet: MixedTransports


### Advanced transport features

Some transports offer advanced features which are not supported by ServiceControl e.g. [RabbitMQ custom routing topologies](/nservicebus/rabbitmq/routing-topology.md#custom-routing-topology).

In this case a Transport Adapter can be used to translate between the customized transport on one side and ServiceControl using the default transport settings on the other side.

Following code shows the configuration of the Transport Adapter in the advanced features scenario:

snippet: AdvancedFeatures

The `UseSpecificRouting` any advanced routing configuration. Notice that this configuration is only applied to the endpoint-side transport.


### Multiple instances of message broker

In some very large systems a single instance of a message broker can't cope with all the traffic. Endpoints can be grouped around broker instances. ServiceControl, however, is limited to a single connection. 

In this case a separate Transport Adapter is deployed for each instance of a broker (e.g. RabbitMQ instance, SQL Server instance or ASB/ASQ namespace) while ServiceControl connects to its own instance. The adapters forward messages between instances.

Following code shows the configuration of the Transport Adapter in the multi-instance scenario:

snippet: MultiInstance

Notice that both adapter configurations use the same connection string for the ServiceControl and a different connection string for the endpoint-side transport.

NOTE: Some transports, such as [SQL Server transport](/nservicebus/sqlserver/), offer a native multi-instance mode. This mode is considered an advanced routing feature and allows to use a single instance of the Transport Adapter even though there are multiple instances of the broker.


## Queue configuration

The Transport Adapter allows to configure the addresses of the forwarded queues: audit, error and control (input queue of ServiceControl). Both endpoint- and ServiceControl-side queues can be configured. 

NOTE: The default values are only suitable in case where both sides of the adapter use different transports or at least different broker instances. In case the adapter runs on a single transport and instance, the queue names on one side need to be altered.

snippet: AuditQueues

snippet: ErrorQueues

snippet: ControlQueues


### Poison

The poison message queue is a special queue which is used when messages cannot be received from the transport because they are corrupted e.g. message headers are malformed.

snippet: PosionQueue

Such messages cannot be forwarded to the error queue because ServiceControl won't be able to receive them. During normal operations NServiceBus system never generates such malformed messages. They can be a generated e.g. by a  misbehaving integration component.


## Failure handling

ServiceControl Transport Adapter uses different failure handling strategies for different types of messages.


### Audits and errors

Audited and failed messages sent by the endpoints to `audit` and `error` queues are subject to *retry forever* policy. This means that the adapter will retry forwarding such message to ServiceControl until it succeeds. 


### Control messages

Control messages, such as heartbeats and custom checks are subject to *best effort* policy which retries each message up to a configured number of times.

snippet: ControlRetries

If the adapter still cannot forward the message, it drops it.


### Retry messages

Retry messages are messages selected for retry processing in their destination endpoint via ServicePulse. Retry messages use immediate retry mechanism similar to control messages but in case of failure they are router back to ServiceControl and reappear in ServicePulse. Following API controls the maximum number of attempts to forward a retry message:

snippet: RetryRetries 


## Message duplication

ServiceControl Transport Adapter can operate in different consistency modes, depending on the transport configuration on both sides. By default the highest supported mode for the transport is used.

If both transports support the [`TransactionScope` transaction mode](https://docs.particular.net/nservicebus/transports/transactions#transactions-transaction-scope-distributed-transaction) the Transport Adapter guarantees to not introduce any duplicates while forwarding messages between the queues. This is very important for the endpoints that can't cope with duplicates and rely on the DTC to ensure *exactly-once* message delivery.

In case at least one of the transport does not support the `TransactionScope` mode or is explicitly configured to a lower mode, the Transport Adapter guarantees *at-least-once* message delivery. This means that the messages won't be lost during forwarding (with the exception of control messages, as described above) but duplicates may be introduced on any side. 

Duplicates are not a problem for ServiceControl because it's business logic is idempotent but the regular business endpoints need to either explicitly deal with duplicates or enable the [Outbox](/nservicebus/outbox/) feature.