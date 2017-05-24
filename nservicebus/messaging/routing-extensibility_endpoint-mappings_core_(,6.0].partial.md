
## MessageEndpointMappings

The routing system can be extended in a static manner (once at startup) by providing custom sources of routing information to enrich or replace the standard routing configuration (`UnicastBusConfig/MessageEndpointMappings` configuration section in `app.config` file).

It can be done either by using a configuration source:

snippet: endpoint-mapping-configurationsource

snippet: inject-endpoint-mapping-configuration-source

or a configuration provider:

snippet: endpoint-mapping-configurationprovider

The `MessageEndpointMappings` collection can be populated based on any external source. It is read during the endpoint start-up, before any messages are sent.

NOTE: The route table is not updated during run-time, even if the contents of the mappings collection change. In case the routing data changes frequently, consider implementing a mechanism that would restart the endpoint when the change is detected.
