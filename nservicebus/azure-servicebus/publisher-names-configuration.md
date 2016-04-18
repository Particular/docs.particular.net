---
title: Publishers name configuration
summary: Configuration mapping between publisher names and message types for Endpoint Oriented Topology
tags:
- Cloud
- Azure
- Transports
reviewed: 2016-04-13
---

When `EndpointOrientedTopology` is selected, mapping between publisher names and message types has to be properly configured at transport level to ensure subscriber receives event messages.  

Mapping can be configured for a specific message type:

snippet: publisher_names_mapping_by_message_type

or by supplying an assembly to scan. 

snippet: publisher_names_mapping_by_assembly

In this case, transport goes through all the assembly types to extract each type that looks as a message, using configured [`message conventions`](/nservicebus/messaging/conventions.md) to identify a message. Then transport registers a mapping between discovered message types and supplied publisher name.

**publisherName** is the name of the endpoint that will publish mapped message.