---
title: Routing system extensibility points
summary: Extend NServiceBus routing with custom route and publisher sources, such as a database or service discovery, and update them at runtime.
reviewed: 2026-10-26
component: Core
related:
 - nservicebus/messaging/routing
---

Extending the [NServiceBus routing subsystem](/nservicebus/messaging/routing.md) with a custom data source makes sense in the following scenarios:

- When centralizing all routing information in a database.
- When dynamically calculating routes using [service discovery](https://en.wikipedia.org/wiki/Service_discovery).
- When routing by a convention based on message type names.

## Applying routing strategies

Routing a message involves the following steps:

- Select the routing strategy based on the [message intent](/nservicebus/messaging/messages-events-commands.md)
- Determine the properties for the selected routing strategy based on the message type, for example, the destination address for a command routed with the [unicast](/transports/types.md#unicast-only-transports) strategy

### Command routing

To extend [command routing](/nservicebus/messaging/routing.md#command-routing), routing extensions can access the routing table from `EndpointConfiguration` or from a [feature](/nservicebus/pipeline/features.md):

snippet: RoutingExtensibility-RouteTableConfig

> [!NOTE]
> To access the `GetSettings` method, include a `using` directive for the `NServiceBus.Configuration.AdvancedExtensibility` namespace.

The routing table can be modified in the feature setup phase or can be passed further, e.g. to a `FeatureStartupTask`, and updated periodically when the source of the routing information changes.

snippet: RoutingExtensibility-StartupTaskRegistration

snippet: RoutingExtensibility-StartupTask

The `sourceKey` parameter is used as a unique key. When `AddOrReplaceRoutes` is called the first time with a given source key, the routes are added to the table. When it is called subsequently, the routes previously registered under that source key are replaced by the new routes.

The routing table API is thread-safe and atomic, meaning either all the changes from the passed collection are successfully applied or none are.

The routing system prevents route ambiguity. If new or replaced routes conflict with existing ones, `AddOrReplaceRoutes` throws an exception and the routing table stays unchanged. The route extension must handle the exception. Usually the safest option is to raise a [critical error](/nservicebus/hosting/critical-errors.md) so the endpoint stops instead of routing messages incorrectly. By default a critical error is only logged, so the endpoint needs a [critical error action](/nservicebus/hosting/critical-errors.md#what-if-i-need-to-override-the-default-behavior) that stops it.

snippet: RoutingExtensibility-TriggerEndpointShutdown

### Event routing

[Event routing](/nservicebus/messaging/routing.md#event-routing) differs depending on the transport capabilities. [Multicast transports](/transports/types.md#multicast-enabled-transports) which support the [Publish-Subscribe](/nservicebus/messaging/publish-subscribe/) pattern natively implement the event routing themselves. Refer to specific transport documentation for details on extensibility points.

Transports without that support rely on NServiceBus core routing for event delivery. The key concept is the publishers collection, which contains information on the logical endpoint that publishes each event. Routing extensions can access the publishers collection from `EndpointConfiguration` or from the `Feature` setup code:

snippet: RoutingExtensibility-Publishers

The `sourceKey` parameter has the same meaning and effect as for the routing table.

The publishers collection is thread-safe, and all operations on that collection are atomic.

## Advanced routing customizations

If there's a need to adjust the routing based on criteria other than the message type, the [routing pipeline stage](/nservicebus/pipeline/steps-stages-connectors.md#stages-outgoing-pipeline-stages) allows routing customization for all messages emitted by the endpoint.

> [!NOTE]
> This intercepts **all** dispatched messages, including messages that aren't NServiceBus message types, such as audit messages and messages sent to the error queue.
