---
title: Publishers name configuration
summary: Configuration mapping between publisher names and message types for Endpoint Oriented Topology
component: ASB
tags:
- Cloud
- Azure
- Transports
reviewed: 2016-04-18
---

When `EndpointOrientedTopology` is selected, a mapping between publisher names and message types has to be properly configured, in order to ensure that subscriber receives event messages.  

The mapping can be configured for specific message type:

snippet: publisher_names_mapping_by_message_type


or for an assembly containing message types:

snippet: publisher_names_mapping_by_assembly

In this case, the transport analyzes all types in the assembly to identify which are messages, according to the configured [`message conventions`](/nservicebus/messaging/conventions.md). Then for each message type, the transport registers a mapping between the type and the publisher name.

In the snippets the **publisherName** is the name of the endpoint that will publish messages.