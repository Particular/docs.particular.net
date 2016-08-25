The routing system can be extended by accessing the APIs via the settings bag. Extending the routing with a custom data source makes sense in following scenarios:
 * When centralizing all routing information in a database
 * When dynamically calculating routes based on endpoint discovery protocol (similar of UDDI)
 * When using a convention based on massage naming

To learn more about implementing a routing extension see the [custom routing sample](/samples/routing/custom/).

## Command routing

Routing extensions can access the route table from `EndpointConfiguration` level:

snippet:RoutingExtensibility-RouteTableConfig

Or from the feature level. In this case the route table can be modified in the feature set up phase or can be passed further e.g. to a `FeatureStartupTask` and updated periodically when the source of routing information changes.

snippet:RoutingExtensibility-StartupTaskRegistration

snippet:RoutingExtensibility-StartupTask

The source parameter is used as a unique key. When the `AddOrReplaceRoutes` is called the first time with a given source, the routes are added to the table. When it is called subsequently, the routes previously registered under that source are replaced by the new routes.

The route table API is thread-safe and atomic, meaning either all the changes from the passed collection or successfully applied or none is.

The routing system prevents route ambiguity. If the new or replaced routes conflict with existing ones, an exception is thrown. It is up to the route extension to deal with that exception but usually it is best practice to trigger the endpoint shutdown to prevent misrouting of messages.

snippet:RoutingExtensibility-TriggerEndpointShutdown 

## Event routing

Event routing differs depending on the transport capabilities. Transports supporting the [Publish-Subscribe](/nservicebus/messaging/publish-subscribe/) pattern natively ({include: transports-multicast}) implement the event routing themselves. Refer to specific transport documentation for details on extensibility points.

Transports without that support ({include: transports-unicast}) rely on NServiceBus core routing for event delivery. To emulate multicast routing on top of a unicast transport NServiceBus uses. The key concept is the collection of publishers. For each event it contains information on the logical endpoint that publishes it. Routing extensions can access the publishers collections from `EndpointConfiguration` or from the `Feature` set up code:

snippet:RoutingExtensibility-Publishers

The source parameter has the same meaning and effect as in the routes collection.

The publishers collection is thread-safe and all operations on that collection are atomic.

## Physical routing

Physical routing is responsible for mapping the destination logical endpoint to the actual transport address (queue name). 

When using a broker transport ({include:transports-broker}), the physical routing is entirely managed by NServiceBus and does not require any configuration.

When using a bus transport ({include:transports-bus}), the physical routing is important because the transport address has to contain the information about the node of the bus that the endpoint is using. In MSMQ each machine runs a single node of the MSMQ system.

Routing extensions can influence the physical routing by modifying the endpoint instances collection. This is especially important for bus transports ({include:transports-bus}) in a dynamically changing environment such as the cloud. Endpoints can be elastically scaled out and in and the routing, to be able to stay in sync, needs to derive the physical information from the current state of the environment, not from a static file.

snippet:RoutingExtensibility-Instances

The source parameter has the same meaning and effect as in the routes collection.

The instances collection is thread-safe. It allows registering multiple instance of a given endpoint. In case there is more than one, message distribution is involved. 

## Message distribution

Every message is always delivered to a single physical instance of the logical endpoint. When scaling out an endpoint with a bus transport ({include:transports-bus}) there are multiple instances of a single logical endpoint registered in the routing system. Each outgoing message has to undergo the distribution process to determine which instance is going to receive this particular message. By default a round-robin algorithm is used to determine the destination. Routing extensions can override this behavior by registering a custom `DistributionStrategy` for a given destination endpoint.

snippet:RoutingExtensibility-Distribution

snippet:RoutingExtensibility-DistributionStrategy

To learn more about creating custom distribution strategies see the [fair distribution sample](/samples/routing/fair-distribution/)