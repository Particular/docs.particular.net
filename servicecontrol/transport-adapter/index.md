---
title: ServiceControl Transport Adapter
summary: How to decouple ServiceControl from an endpoint's transport
component: SCTransportAdapter
reviewed: 2021-02-22
related:
 - nservicebus/dotnet-templates
 - samples/servicecontrol/adapter-mixed-transports
 - samples/servicecontrol/adapter-asb-multi-namespace
---

The ServiceControl Transport Adapter decouples ServiceControl from the specifics of the business endpoint's transport to support scenarios where the endpoint's transport uses physical routing features [not compatible with ServiceControl](/servicecontrol/transport-adapter/incompatible-features.md) or where endpoints use mixed transports or multiple instances of a message broker.


## Usage scenarios

The ServiceControl Transport Adapter can be useful in three scenarios:


### Mixed transports

Some endpoints in the system use a different transport than the rest of the system but should report to the same instance of ServiceControl. Some examples include integration with external parties, or using a specific transport feature in a subset of endpoints (e.g. MSMQ store-and-forward for occasionally connected endpoints).

In these cases, a transport adapter can be used for the subset of endpoints that use a different transport. This allows management of the whole system from a single instance of ServicePulse.

The following code shows the configuration of a transport adapter in a mixed transport scenario:

snippet: MixedTransports

NOTE: If the adapter is run in conjunction with the NServiceBus endpoint, even though the adapter's transport configuration is similar to the endpoint transport (e.g. ConnectionStrings, etc.), it still needs to be done for both.

### Advanced transport features

Some transports offer advanced features which are not supported by ServiceControl.

In this case, a transport adapter can be used to translate between the customized transport on one side and ServiceControl using the default transport settings on the other side.

The following code shows the configuration of the transport adapter in the advanced features scenario:

snippet: AdvancedFeatures

In the snippet above, `UseSpecificRouting` represents any advanced routing configuration applicable for a given transport that is not compatible with ServiceControl. Notice that this configuration is applied only to the endpoint-side transport.


### Multiple instances of a message broker

In some very large systems, a single instance of a message broker can't cope with the traffic. Endpoints can be grouped around broker instances. However, ServiceControl is limited to a single connection.

In this case, a separate transport adapter is deployed for each instance of a broker (e.g. SQL Server instance, Azure ServiceBus namespace, etc) while ServiceControl connects to its own instance. The adapters forward messages between instances.

The following code shows the configuration of the transport adapter in the multi-instance scenario:

snippet: MultiInstance

Notice that both adapter configurations use the same connection string for ServiceControl and a different connection string for the endpoint-side transport.


## How it works

The transport adapter uses two sets of queues, one on each side of the communication. Each set consists of three queues that define [ServiceControl's API](/servicecontrol/servicecontrol-instances/): audit, error, and input/control.


### Heartbeats and other control messages

Heartbeats, custom check status notifications, and other control messages arrive at the adapter's input/control queue. They are then moved to ServiceControl's input queue (named after the ServiceControl instance name). If a problem occurs (e.g. the destination database is down), the forward attempts are repeated a certain configurable number of times (5 by default), at which time messages are dropped to prevent the queue from growing indefinitely.

Control messages, such as heartbeats and custom checks, are subject to a *best effort* policy which retries each message up to a configured number of times.

snippet: ControlRetries

If the adapter still cannot forward the message, it will be dropped.


### Audits

The audit messages arrive at adapter's audit queue. They are then moved to the audit queue of the ServiceControl instance. 

Audit messages are subject to a *retry forever* policy; the adapter will retry forwarding audit messages to ServiceControl until they succeed.


### Retries

If a message fails all recoverability attempts in a business endpoint, it is moved to the error queue of the adapter. The adapter enriches the message by adding a `ServiceControl.RetryTo` header pointing to the adapter's retry queue. Then the message is moved to the error queue of ServiceControl and ingested into the ServiceControl RavenDB store. 

When retrying, ServiceControl looks for a `ServiceControl.RetryTo` header. If the header exists, ServiceControl sends the message to the queue from that header instead of the ultimate destination.

The adapter then picks up the message and forwards it to the destination using its endpoint-facing transport.

Failed messages sent by the endpoints to the error queue are subject to a *retry forever* policy. This means that the adapter will retry forwarding these messages to ServiceControl until they succeed.

The transport adapter will retry delivering failed messages selected for retry for the configured number of times. If messages processing still fails, the messages will be routed back to ServiceControl and will reappear in ServicePulse. The following API controls the maximum number of attempts to forward a retry message:

snippet: RetryRetries

### ServiceControl events

[ServiceControl events](/servicecontrol/contracts.md) are not supported by the transport adapter. The endpoint that handles these events must be configured in such a way that it can directly communicate with ServiceControl. Such an endpoint _should not_ process any other message types.


## Queue configuration

The transport adapter allows configuration of the addresses of the forwarded queues: audit, error, and control (input queue of ServiceControl). Both endpoint- and ServiceControl-side queues can be configured.

NOTE: The default values are suitable only in cases where both sides of the adapter use different transports or at least different broker instances. In case the adapter runs on a single transport and instance, the queue names on one side must be altered.


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

These messages cannot be forwarded to the error queue because ServiceControl won't be able to receive them. During normal operations NServiceBus never generates such malformed messages. They can be a generated, for example, by a misbehaving integration component. The poison message queue should be monitored using platform-specific tools available for the messaging technology used in the solution.


## Life cycle and hosting

Transport Adapter is a library package that is hosting-agnostic. In a production scenario, the adapter should be hosted either as a Windows Service or via a cloud-specific hosting mechanism (e.g. Azure Worker Role).

The [Transport Adapter `dotnet new` template](/nservicebus/dotnet-templates.md) makes it easier to create a Windows Service host for the transport adapter. Details on how to install the adapter as a service are outlined in [Windows Service Installation](/nservicebus/hosting/windows-service.md).

Regardless of the hosting mechanism, Transport Adapter follows the same life cycle, which the following snippet demonstrates. The `Start` and `Stop` methods must be bound to host-specific events (e.g. Windows Service start up callback).

snippet: Lifecycle


## Message duplication

ServiceControl Transport Adapter can operate in different consistency modes depending on the transport configuration on both sides. By default, the highest supported mode for the transport is used.

If both transports support the [`TransactionScope` transaction mode](/transports/transactions.md#transactions-transaction-scope-distributed-transaction) the transport adapter guarantees not to introduce any duplicates while forwarding messages between the queues. This is very important for endpoints that can't cope with duplicates and rely on the DTC to ensure *exactly-once* message delivery.

If at least one of the transports does not support the `TransactionScope` mode, or is explicitly configured to a lower mode, the transport adapter guarantees *at-least-once* message delivery. This means that messages won't be lost during forwarding (with the exception of control messages, as described above), but duplicates may be introduced on any side.

Duplicates are not an issue for ServiceControl because its business logic is idempotent. However, other business endpoints must either explicitly deal with duplicates or enable the [outbox](/nservicebus/outbox/) feature.
