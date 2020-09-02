---
title: Publishers name configuration
summary: Configuration mapping between publisher names and event types for the endpoint-oriented topology
component: ASB
versions: "[7,)"
reviewed: 2020-09-02
redirects:
 - nservicebus/azure-service-bus/publisher-names-configuration
 - transports/azure-service-bus/publisher-names-configuration
---

include: legacy-asb-warning

When the [`EndpointOrientedTopology`](/transports/azure-service-bus/legacy/topologies.md#versions-7-and-above-endpoint-oriented-topology) is selected, a mapping between publisher names and event types has to be properly configured, in order to ensure that subscriber can receive event messages. 

The mapping can be configured for a specific event type:

snippet: publisher_names_mapping_by_message_type


or for an assembly containing multiple event types:

snippet: publisher_names_mapping_by_assembly

In the latter case, the transport analyzes all types in the assembly to identify which are events, using the `IEvent` interface or the configured `DefiningEventsAs()` [message convention](/nservicebus/messaging/conventions.md). Then for each event type, the transport registers a mapping between the type and the publisher name.

In the snippets the **publisherName** is the name of the endpoint that will publish messages.
