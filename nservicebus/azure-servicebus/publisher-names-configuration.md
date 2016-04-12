---
title: Publishers name configuration
summary: Configuration mapping between publisher names and message types for Endpoint Oriented Topology
tags:
- Cloud
- Azure
- Transports
---

`ITopology` interface exposes a property (`NeedsMappingConfigurationBetweenPublishersAndEventTypes`) to instruct transport environment if mapping must be configured. Custom topologies have to properly configure this property.   
Configuration is disabled for topologies that don't support mapping. If `RegiterPublisherForType` or `RegisterPublisherForAssembly` methods are called during configuration with a toplogy that doesn't support mapping, transport raises an `InvalidOperationException`.   
   
When Endpoint Oriented Topology is selected, mapping between publisher names and message types has to be properly configured at transport level to ensure that message routing works as expected.  
Mapping can be configured for a specific message type::

snippet: publisher_names_mapping_by_message_type

or by supplying an assembly to scan. Configured `Conventions` will be used to extract messages:

snippet: publisher_names_mapping_by_assembly

**publisherName** is the name of the endpoint that will deliver mapped message.
  



