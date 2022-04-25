---
title: Bridge configuration options
summary: Configuration options for the bridge.
component: Bridge
reviewed: 2022-04-01
---

## Hosting

The `NServiceBus.Transport.Bridge` is hosted via the [.NET Generic Host](https://docs.microsoft.com/en-us/dotnet/core/extensions/generic-host) which takes lifecycle management, configuration, logging etc.

snippet: generic-host

Use the overload with a [`HostBuilderContext`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.hostbuildercontext) to get access to the [`IConfiguration` API](https://docs.microsoft.com/en-us/dotnet/core/extensions/configuration) and other host related details.

snippet: generic-host-builder-context

## Registering endpoints

Each logical endpoint that needs to be available to endpoints that are on another transport, is required to be registered with the bridge. The endpoint is registered on the transport it is running on. The bridge will then create a proxy on each transport that needs to be bridged.

snippet: endpoint-registration

## Registering publishers

When NServiceBus discovers a message handler in an endpoint for an event, it automatically subscribes to this event on the transport. The publisher itself is not aware of this and does not need to receive a notification for subscribers registration to an event. This is a problem for the bridge.

If an endpoint subscribes to an event, the bridge does need to become aware, as it needs to register the exact same subscription on the transports it's bridging. As the bridge is completely unaware of any subscriptions, the bridge needs to be configured to mimic the behavior of the endpoints.

This results in duplicating subscriptions for any endpoint that subscribes to an event. The endpoint that publishes the event needs also to be configured.

snippet: register-publisher

### Events

It is possible to reference message assemblies and use `typeof()` for type-safety when registering publishers.

If messages implement the `IEvent` interface, the message assembly references NServiceBus. When different versions of NServiceBus are referenced by both the message assembly and the bridge, this will result in compile-time exceptions. It is then an option to register the fully-qualified name of an event as a string. As a result no message assemblies will need to be referenced that can cause conflicts during compile-time.

## Provisioning queues

The bridge will by default **not** create any queues for the endpoints that it proxies to not require elevated priviledges at runtime.

TBD: What guidance or tooling if any should be provided around what exact queues and the names of those queues

The queues can be created using on of the following methods:

- Provision them manually using the tooling provided by the queuing system
- Use the queue creation tooling provided by Particular Software if such exists for the transports use. See the [individual transports documentation](/transports/) for more details.
- Configure the Bridge to auto created queues (see below)

### Automatic queue provisioning

NOTE: This option requires the Bridge to have administrative priviledges for the queing systems used and is not recommended for production scenarios.

To enable automatic queue creation configure the Bridge as follows:

snippet: auto-create-queues

## Performance tuning

The Bridge will move messages using [the same default concurrency as NServiceBus endpoints](/nservicebus/operations/tuning.md#configuring-concurrency-limit) which is `max(Number of logical processors, 2)`.

Customizing the concurrency level can be done using the followig configuration:

snippet: custom-concurrency

## Recoverability

Should a message fail to be transfered to the target transport the following recoverability actions will be taken:

1. Three immediate retries will be performed to make sure that the problem isn't transient
1. Should the retries fail the message will be moved to the bridge error queue

### Error queue

The error queue is defaulted to `bridge.error`. Note that the default `error` queue used by other platform components is not used to enable bridiging of the system wide error queue since its not allowed to use a bridged queue as the error queue. See the documentation around briding platform queues for more details. TBD: link

To configure a different error queue using the following configuration:

snippet: custom-error-queue

## Auditing

The bridge will attach a new `NServiceBus.Bridge.Transfer` as a message is transfered between transports. 

The value of the header is `{source-transport-name}->{target-transport-name}`, example:

`msmq->sqlserver`

### Configuring transport name3

The bridge will default the transport name based on the type of transport being used. This means that when bridging transports of the **same type** each transport needs to be given a unique name using the following configuration:

snippet: custom-transport-name
