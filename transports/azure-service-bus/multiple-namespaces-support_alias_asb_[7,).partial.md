Versions 7 and above of the Azure Service Bus transport it is also possible to provide an alias for namespaces, and use alias instead of connection string value; in all of these places.

snippet: namespace_routing_send_options_named

This requires namespace alias and connection string to be registered using the `NamespaceRouting()` API.

snippet: namespace_routing_registration