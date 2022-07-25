---
title: Bridge configuration options
summary: Configuration options for the transport bridge.
component: Bridge
reviewed: 2022-04-01
---

## Hosting

The `NServiceBus.Transport.Bridge` is hosted via the [.NET Generic Host](https://docs.microsoft.com/en-us/dotnet/core/extensions/generic-host) which takes care of life cycle management, configuration, and logging, among other things.

snippet: generic-host

Use the overload that accepts a [`HostBuilderContext`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.hostbuildercontext) to get access to the [`IConfiguration` API](https://docs.microsoft.com/en-us/dotnet/core/extensions/configuration) and other host related details.

snippet: generic-host-builder-context

## Registering endpoints

If a logical endpoint communicates with other endpoints that use a different transport, it must be registered with the bridge. Endpoints are registered with the bridge on the transport they run on. The bridge then creates a proxy endpoint on each transport that needs to be bridged.

snippet: endpoint-registration

## Registering publishers

When NServiceBus discovers a message handler in an endpoint for an event, it automatically subscribes to this event on the transport. The publisher itself is not aware of this and does not need to receive a notification for subscribers registering for an event. This represents a challenge for the bridge.

If an endpoint subscribes to an event, the bridge must be made aware of this subscription since it must register the same subscription on the transports it's bridging. As the bridge is unaware of any subscriptions, the bridge must be configured to mimic the behavior of the endpoints.

The result is duplicate subscriptions for any endpoint that subscribes to an event. The endpoint that publishes the event must be configured as well.

snippet: register-publisher

### Events

It is possible to reference message assemblies and use `typeof()` for type-safety when registering publishers. When this option isn't available (for example if the shared message assembly references a different version of NServiceBus than the bridge), the fully-qualified name of an event can be used instead. In this scenario, no message assemblies need to be referenced, reducing the likelihood of conflicts during compile-time.

## Provisioning queues

By default, the bridge does **not** create queues for the endpoints that it proxies. This is done so that elevated privileges (which are often needed to create the queues) are not required at runtime.

The queues can be created using one of the following methods:

- Provision them manually using the tooling provided by the queuing system
- Use the queue creation tooling provided by Particular Software if one exists for the transports being used. See the [individual transports documentation](/transports/) for more details.
- Configure the bridge to create queues automatically as described in the next section

### Automatic queue provisioning

NOTE: This option requires the bridge to have administrative privileges for the queuing systems used and is not recommended for production scenarios.

To enable automatic queue creation configure the bridge as follows:

snippet: auto-create-queues

## Custom queue address

The bridge provides the ability to adjust the address for the queue of incoming messages. 

NOTE: With MSMQ endpoints that run on a different server than the bridge, it is mandatory to provide the address of the queue that messages should be forwarded to.

snippet: custom-address

## Recoverability

If a message fails while it is being transferred to the target transport, the following recoverability actions are taken:

1. Three immediate retries are performed to make sure that the problem isn't transient
1. If the retries fail, the message is moved to the bridge error queue

### Error queue

The error queue used by the bridge is named `bridge.error` by default. Note that the default `error` queue used by other platform components can not be used to enable bridging of the system-wide error queue since a bridged queue may not be used as the error queue. See the documentation around [bridging platform queues](#bridging-platform-queues) for more details.

To configure a different error queue using the following configuration:

snippet: custom-error-queue

Messages moved to the error queue have the [`NServiceBus.FailedQ`](/nservicebus/messaging/headers.md#error-forwarding-headers-nservicebus-failedq) header set to allow scripted retries. Refer to the documentation for the [various transports](/transports) for more details in how to perform retries.

## Auditing

The bridge attaches a new `NServiceBus.Bridge.Transfer` header while a message is transferred between transports.

The value of the header is `{source-transport-name}->{target-transport-name}`. For example: `msmq->sqlserver`. This header provides traceability for a message as it moves through the bridge.

### Configuring transport

By default, the bridge assigns a transport name based on the type of transport being used. This means that when bridging transports of the **same type** each transport must be given a unique name using the following configuration:

snippet: custom-transport-name

## Bridging platform queues

The bridge can be used to enable a single ServiceControl installation to manage and monitor endpoints on all bridged transports.

Configure the bridge as follows to enable platform bridging:

snippet: platform-bridging

### Audit queue

Special considerations must be taken for the audit queue due to potentially high volume message. Consider installing a [dedicated ServiceControl audit instance](/servicecontrol/audit-instances/) for each bridged transport to make audit ingestion more efficient.