---
title: Routing system extensibility points
summary: How NServiceBus routing can be extended
reviewed: 2025-04-03
component: Core
related:
 - nservicebus/messaging/routing
---

Extending the [NServiceBus routing subsystem](/nservicebus/messaging/routing.md) with a custom data source makes sense in following scenarios:

 * When centralizing all routing information in a database.
 * When dynamically calculating routes based on endpoint discovery protocol (similar to [UDDI](https://en.wikipedia.org/wiki/Web_Services_Discovery)).
 * When using a convention based on message naming.

## Applying routing strategies

Routing a message involves the following steps:

- Select the routing strategy based on the [message intent](/nservicebus/messaging/messages-events-commands.md)
- Determine the specific properties for the selected routing strategy based on the message type. e.g. as destination address in case of a command/[unicast](/transports/types.md#unicast-only-transports) strategy

### Command routing

To extend [command routing](/nservicebus/messaging/routing.md#command-routing), routing extensions can access the routing table on the `EndpointConfiguration` level or from the feature level:

snippet: RoutingExtensibility-RouteTableConfig

> [!NOTE]
> To access the `GetSettings` method, include a `using` directive for the `NServiceBus.Configuration.AdvancedExtensibility` namespace.

The routing table can be modified in the feature set up phase or can be passed further, e.g. to a `FeatureStartupTask`, and updated periodically when the source of the routing information changes.

snippet: RoutingExtensibility-StartupTaskRegistration

snippet: RoutingExtensibility-StartupTask

The source parameter is used as a unique key. When `AddOrReplaceRoutes` is called the first time with a given source key, the routes are added to the table. When it is called subsequently, the routes previously registered under that source key are replaced by the new routes.

The routing table API is thread-safe and atomic, meaning either all the changes from the passed collection are successfully applied or none are.

The routing system prevents route ambiguity. If new or replaced routes conflict with existing ones, an exception is thrown. It is up to the route extension to deal with the exception but usually it is a good practice to trigger the endpoint shutdown preventing the incorrect routing of messages.

snippet: RoutingExtensibility-TriggerEndpointShutdown

### Event routing

[Event routing](/nservicebus/messaging/routing.md#event-routing) differs depending on the transport capabilities. [Multicast transports](/transports/types.md#multicast-enabled-transports) which support the [Publish-Subscribe](/nservicebus/messaging/publish-subscribe/) pattern natively implement the event routing themselves. Refer to specific transport documentation for details on extensibility points.

Transports without that support rely on NServiceBus core routing for event delivery. The key concept is the publishers collection, which contains information on the logical endpoint that publishes each event. Routing extensions can access the publishers collection from `EndpointConfiguration` or from the `Feature` setup code:

snippet: RoutingExtensibility-Publishers

The source parameter has the same meaning and effect as in the routes collection.

The publishers collection is thread-safe, and all operations on that collection are atomic.

## Advanced routing customizations

If there's a need to adjust the routing based on criteria other than the message type, the [routing pipeline stage](/nservicebus/pipeline/steps-stages-connectors.md#stages-outgoing-pipeline-stages) allows routing customization for all messages emitted by the endpoint.

> [!NOTE]
> Be aware that this intercepts **all** messages that are dispatched, including messages that are not known NServiceBus message types, e.g. an audit message.
