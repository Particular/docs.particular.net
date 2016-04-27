---
title: Azure Service Bus Transport Upgrade Version 6 to 7
summary: Instructions on how to upgrade Azure Service Bus Transport Version 6 to 7.
reviewed: 2016-04-19
tags:
 - upgrade
 - migration
related:
- nservicebus/azure-service-bus
- nservicebus/upgrades/5to6
---


## New Configuration API

In Versions 6 and below the Azure Service Bus transport was configured using an XML configuration section called `AzureServiceBusQueueConfig`. This section has been removed in favor of a more granular, code based configuration API.

The new configuration API is accessible through extension methods on the `UseTransport<AzureServiceBusTransport>()` extension point in the endpoint configuration. Refer to the [Full Configuration Page](/nservicebus/azure-service-bus/configuration/configuration.md) for more details.

snippet:AzureServiceBusTransportWithAzure


### Setting The Connection String

Setting the connection string can still be done using the `ConnectionString` extension method:

snippet:setting_asb_connection_string


### Setting Entity Property Values

The other configuration properties found on `AzureServiceBusQueueConfig` were related to the configuration of Azure Service Bus entities used under the hood. Even though the name implied differently, these settings were not only applied to queues, but also to topics and subscriptions. 

In the new configuration API the settings for queues, topics and subscriptions can be configured individually using the `Queues()`, `Topics()` and `Subscriptions()` extension points. 

For example the lock duration on a queue can be configured using the `LockDuration` setting:

snippet:setting_queue_properties

and the size of the topics can be configured using the `MaxSizeInMegabytes` setting:

snippet:setting_topic_properties


### Default value changes

The default values of the following settings have been changed:

 * `BatchSize`, which had a default value of 1000, is replaced by `PrefetchCount` with a default value of 200.
 * `MaxDeliveryCount` changed from 6 to 10.


## [Topology](/nservicebus/azure-service-bus/topologies/) is mandatory

In Versions 7 and above the topology selection is mandatory:

snippet:topology-selection-upgrade-guide


When the `EndpointOrientedTopology` is selected, it is also necessary to configure [publisher names](/nservicebus/azure-service-bus/publisher-names-configuration.md), in order to ensure that subscribers receive event messages:

snippet:publisher_names_mapping_upgrade_guide

## Keep connection string safe

In version 7 and above, to avoid sharing sensitive data contained into connection strings, it's possibile to enable `UseNamespaceNameInsteadOfConnectionString` feature: 

snippet: enable_use_namespace_name_instead_of_connection_string

Refer to the [Keep connection string safe page](/nservicebus/azure-service-bus/keep-connectionstring-safe.md) for more details.

  