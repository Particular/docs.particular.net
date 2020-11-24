---
title: Azure Service Bus Transport (Legacy) Upgrade Version 6 to 7
summary: Instructions on how to upgrade Azure Service Bus Transport Version 6 to 7.
reviewed: 2020-11-23
component: ASB
related:
 - transports/azure-service-bus
 - nservicebus/upgrades/5to6
redirects:
 - nservicebus/upgrades/asb-6to7
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---


## Topology is mandatory

In Versions 7 and above the [topology selection](/transports/azure-service-bus/legacy/topologies.md) is mandatory:

snippet: topology-selection-upgrade-guide

The [`EndpointOrientedTopology`](/transports/azure-service-bus/legacy/topologies.md#versions-7-and-above-endpoint-oriented-topology) is backward compatible with versions 6 and below of the transport. The [`ForwardingTopology`](/transports/azure-service-bus/legacy/topologies.md#versions-7-and-above-forwarding-topology) is the recommended option for new projects.

When selecting `EndpointOrientedTopology`, it is also necessary to configure [publisher names](/transports/azure-service-bus/legacy/publisher-names-configuration.md), in order to ensure that subscribers are subscribed to the correct publisher:

snippet: publisher_names_mapping_upgrade_guide

For more details on topologies refer to the [Azure Service Bus Transport Topologies](/transports/azure-service-bus/legacy/topologies.md) article.


## Sanitization

Azure Service Bus entities have path and naming rules that limit the allowed characters and maximum length.  NServiceBus derives entity names from endpoint and event names; these derived names may fail Azure Service Bus validation. Sanitization allows to modify entity names to meet the namning rules of the broker.

In Versions 6 and below sanitization was performed by default and the MD5 algorithm was used to truncate entity names. In Versions 7 and above, the sanitization has to configured explicitly.

In order to maintain backward compatibility, [register a custom sanitization strategy](/transports/azure-service-bus/legacy/sanitization.md#automated-sanitization-backward-compatibility-with-versions-6-and-below).

In version 6.4.0 `NamingConventions` class was introduced to customize sanitization. The class is obsoleted. Instead, implement a [custom sanitization strategy](/transports/azure-service-bus/legacy/sanitization.md#sanitization).


## New Configuration API

In Versions 6 and below the transport was configured using the `AzureServiceBusQueueConfig`configuration section. This section has been removed in favor of a more granular code based configuration API.

The new API is accessible through the `.UseTransport<AzureServiceBusTransport>()` extension point. Refer to the [Full Configuration Page](/transports/azure-service-bus/legacy/configuration/full.md) for more details.


### Configuring a connection string

Setting a connection string is still supported using the `ConnectionString` extension method:

snippet: 6to7_setting_asb_connection_string


### Default value changes

The default values for the following settings have changed:

 * `BatchSize`, which had a default value of 1000, is replaced by `PrefetchCount` with a default value of 200. 
 * `MaxDeliveryCount` is set to number of immediate retries + 1. For system queues the value has changed from 6 to 10.

For more details refer to the [ASB Batching](/transports/azure-service-bus/legacy/batching.md) and [ASB Retry behavior](/transports/azure-service-bus/legacy/retries.md) articles. 

### Setting Entity Property Values

The other configuration properties found on `AzureServiceBusQueueConfig` were related to the configuration of Azure Service Bus entities. Even though the name implied differently, these settings were not only applied to queues, but also to topics and subscriptions.

In the new configuration API the settings for queues, topics and subscriptions can be configured individually using the `Queues()`, `Topics()` and `Subscriptions()` extension points.

For example the lock duration on a queue can be configured using the `LockDuration` setting:

snippet: setting_queue_properties

and the size of the topics can be configured using the `MaxSizeInMegabytes` setting:

snippet: setting_topic_properties


## [Securing Credentials](/transports/azure-service-bus/legacy/securing-connection-strings.md)

include: asb-credential-warning

In order to enhance security and to avoid sharing sensitive information enable the `UseNamespaceNameInsteadOfConnectionString` feature using the following steps:

 * Upgrade all endpoints to Version 7 or above. Previous versions of the transport aren't able to understand namespace names instead of connection strings.
 * After the above has been done and all endpoints have been deployed switch on the `UseNamespaceNameInsteadOfConnectionString` feature and re-deploy.


## BrokeredMessage conventions

In versions 6 and below, `BrokeredMessage` conventions were specified using `BrokeredMessageBodyConversion` class. In versions 7 and above, it has been replaced by [configuration API](/transports/azure-service-bus/legacy/brokered-message-creation.md).
