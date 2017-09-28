Versions 8 and above of the Azure Service Bus transport provide a more convenient way of sending messages over multiple namespaces that does not required to explicitely specify the destination.

To leverage that capability the namespace alias and connection string need to be registered using the `NamespaceRouting()` API. Furthermore, the endpoint name needs to be registered on the namespace that it belongs to.

snippet: namespace_routing_endpoint_registration

With that in place a normal send operation can be issued without needing to specify the destination with an alias or a connection string.

snippet: namespace_routing_send_registered_endpoint

### cross namespace subscriptions

Similar to cross namespace sending it is also possible to subscribe to registered endpoints outside the topology by registering the publisher explicitly.

snippet: namespace_subscription_registered_endpoint

NOTE: This feature is only available for the `EndpointOrientedTopology` as the `ForwardingTopology` relies on the Azure Service Bus forwarding feature that does not allow forwarding between entities in different namespaces.