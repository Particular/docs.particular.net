---
title: Bridge configuration options
summary: Configuration options for the bridge.
component: Bridge
reviewed: 2022-04-01
---

## Generic host

The `NServiceBus.Transport.Bridge` needs to Generic Host and the [`Microsoft.Extensions.Hosting`](https://www.nuget.org/packages/Microsoft.Extensions.Hosting.WindowsServices/) NuGet package to host the bridge and manage the component's life cycle.

Configuration of the bridge is then achieved as follows:

snippet: generic-host

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