---
title: ServiceControl Transport Adapter
summary: How to decouple ServiceControl from endpoint's transport
component: SCTransportAdapter
reviewed: 2017-06-28
related:
 - samples/servicecontrol/adapter-sqlserver-multi-schema
 - samples/servicecontrol/adapter-sqlserver-multi-instance
 - samples/servicecontrol/adapter-mixed-transports
 - samples/adapter-asb-multi-namespace
 - samples/adapter-rabbitmq-different-topologies
---

The ServiceControl Transport Adapter decouples ServiceControl from the specifics of the business endpoint's transport to support scenarios where the endpoint's transport uses physical routing features [not compatible with ServiceControl](/servicecontrol/transport-adapter/incompatible-features.md) or where endpoints use mixed transports or multiple instances of a message broker.


## Usage scenarios

The ServiceControl Transport Adapter can be useful in three scenarios:


### Mixed transports

Some endpoints in the system use a different transport than the rest of the system but should report to the same instance of ServiceControl. Some examples include integration with external parties, or using a specific transport feature in a subset of endpoints (e.g. MSMQ store-and-forward for occasionally connected endpoints).

In these cases, a transport adapter can be used for the subset of endpoints that use a different transport. This allows management of the whole system from a single instance of ServicePulse.

The following code shows the configuration of the Transport Adapter in a mixed transport scenario:

snippet: MixedTransports


### Advanced transport features

Some transports offer advanced features which are not supported by ServiceControl (e.g. [RabbitMQ custom routing topologies](/transports/rabbitmq/routing-topology.md#custom-routing-topology)).

In this case a transport adapter can be used to translate between the customized transport on one side and ServiceControl using the default transport settings on the other side.

The following code shows the configuration of the transport adapter in the advanced features scenario:

snippet: AdvancedFeatures

In the snippet above `UseSpecificRouting` represents any advanced routing configuration applicable for a given transport that is not compatible with ServiceControl. Notice that this configuration is only applied to the endpoint-side transport.


### Multiple instances of a message broker

In some very large systems a single instance of a message broker can't cope with the traffic. Endpoints can be grouped around broker instances. ServiceControl, however, is limited to a single connection.

In this case a separate transport adapter is deployed for each instance of a broker (e.g. RabbitMQ instance, SQL Server instance or ASB/ASQ namespace) while ServiceControl connects to its own instance. The adapters forward messages between instances.

The following code shows the configuration of the transport adapter in the multi-instance scenario:

snippet: MultiInstance

Notice that both adapter configurations use the same connection string for ServiceControl and a different connection string for the endpoint-side transport.

NOTE: Some transports, such as [SQL Server transport](/transports/sql/), offer a native multi-instance mode. This mode is considered an advanced routing feature and allows use of a single instance of the transport adapter even though there are multiple instances of the broker.


## How it works

The transport adapter uses two sets of queues, one on each side. Each set consists of three queues that define [ServiceControl's API](/servicecontrol/servicecontrol-instances/): audit, error and input/control.


### Heartbeats and other control messages

Heartbeats, custom check status notifications and other control messages arrive at the adapter's input/control queue. They are then moved to ServiceControl's input queue (named after ServiceControl instance name). In case of problems (e.g. destination database being down) the forward attempts are repeated configurable number of times (5 by default) after which messages are dropped to prevent the queue from growing indefinitely.

Control messages, such as heartbeats and custom checks, are subject to a *best effort* policy which retries each message up to a configured number of times.

snippet: ControlRetries

If the adapter still cannot forward the message, it will be dropped.


### Audits

The audit messages arrive at adapter's audit queue. They are then moved to the audit queue of ServiceControl instance. 

Audit messages are subject to a *retry forever* policy. This means that the adapter will retry forwarding such message to ServiceControl until it succeeds.


### Retries

If a message fails all recoverability attempts in a business endpoint, it is moved to the error queue of the adapter. The adapter enriches the message by adding `ServiceControl.RetryTo` header pointing to the adapter's retry queue. Then the message is moved to the error queue of ServiceControl and ingested into ServiceControl RavenDB store. 

When retrying, ServiceControl looks for `ServiceControl.RetryTo` header and, if it finds it, it sends the message to the queue from that header instead of the ultimate destination.

The adapter picks up the message and forwards it to the destination using its endpoint-facing transport.

Failed messages sent by the endpoints to the error queue are subject to a *retry forever* policy. This means that the adapter will retry forwarding such message to ServiceControl until it succeeds.

Transport Adapter will attempt to retry delivering failed messages selected for retry for the configured number of times. If messages processing still fails, the messages will be routed back to ServiceControl and will reappear in ServicePulse. Following API controls the maximum number of attempts to forward a retry message:

snippet: RetryRetries

### ServiceControl events

The [ServiceControl events](/servicecontrol/contracts.md) are not supported by the transport adapter. The endpoint that handles these events has to be configured in such a way that it can directly communicate with ServiceControl. It is advised that such an endpoint does not process any other message types.


## Queue configuration

The transport adapter allows configuration of the addresses of the forwarded queues: audit, error and control (input queue of ServiceControl). Both endpoint- and ServiceControl-side queues can be configured.

NOTE: The default values are only suitable in cases where both sides of the adapter use different transports or at least different broker instances. In case the adapter runs on a single transport and instance, the queue names on one side need to be altered.


### Audit queues

Default: `audit`

snippet: AuditQueues


### Error queues

Default: `error`

snippet: ErrorQueues


### Control queues

Default: `Particular.ServiceControl`

snippet: ControlQueues


### Poison message queue

The poison message queue is a special queue which is used when messages cannot be received from the transport because they are corrupted (e.g. message headers are malformed).

Default: `poison`

snippet: PoisonQueue

Such messages cannot be forwarded to the error queue because ServiceControl won't be able to receive them. During normal operations NServiceBus never generates such malformed messages. They can be a generated, for example, by a misbehaving integration component. The poison message queue should be monitored using platform-specific tools available for the messaging technology used in the solution.


## Life cycle and hosting

Transport Adapter is a library package that is hosting-agnostic. In a production scenario the adapter should be hosted either as a Windows Service or via a cloud-specific hosting mechanism (e.g. Azure Worker Role).

The [Transport Adapter `dotnet new` template](template.md) makes it easier to create a Windows Service host for the Transport Adapter.

Regardless of the hosting mechanism, the Transport Adapter follows the same life cycle. The following snippet demonstrates it. The `Start` and `Stop` methods need to be bound to host-specific events (e.g. Windows Service start up callback).

snippet: Lifecycle


## Message duplication

ServiceControl Transport Adapter can operate in different consistency modes depending on the transport configuration on both sides. By default, the highest supported mode for the transport is used.

If both transports support the [`TransactionScope` transaction mode](/transports/transactions.md#transactions-transaction-scope-distributed-transaction) the transport adapter guarantees not to introduce any duplicates while forwarding messages between the queues. This is very important for the endpoints that can't cope with duplicates and rely on the DTC to ensure *exactly-once* message delivery.

In case at least one of the transports does not support the `TransactionScope` mode or is explicitly configured to a lower mode, the transport adapter guarantees *at-least-once* message delivery. This means that the messages won't be lost during forwarding (with the exception of control messages, as described above) but duplicates may be introduced on any side.

Duplicates are not a problem for ServiceControl because its business logic is idempotent but the regular business endpoints need to either explicitly deal with duplicates or enable the [Outbox](/nservicebus/outbox/) feature.
