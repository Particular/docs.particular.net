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

In Versions 7 and above the topology selection is mandatory:

```csharp
// For Azure Service Bus Transport (Legacy) version 10.x
var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();

transport.UseForwardingTopology();
// OR
transport.UseEndpointOrientedTopology();

// For Azure Service Bus Transport (Legacy) version 9.x
var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();

transport.UseForwardingTopology();
// OR
transport.UseEndpointOrientedTopology();

// For Azure Service Bus Transport (Legacy) version 8.x
var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();

transport.UseForwardingTopology();
// OR
transport.UseEndpointOrientedTopology();

// For Azure Service Bus Transport (Legacy) version 7.x
var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();

transport.UseForwardingTopology();
// OR
transport.UseEndpointOrientedTopology();
```

The `EndpointOrientedTopology` is backward compatible with versions 6 and below of the transport. The `ForwardingTopology` is the recommended option for new projects.

When selecting `EndpointOrientedTopology`, it is also necessary to configure publisher names, in order to ensure that subscribers are subscribed to the correct publisher:

```csharp
// For Azure Service Bus Transport (Legacy) version 10.x
var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
var topology = transport.UseEndpointOrientedTopology();

topology.RegisterPublisher(typeof(MyMessage), "publisherName");
// OR
var messagesAssembly = Assembly.LoadFrom("path/to/assembly/containing/messages");
topology.RegisterPublisher(messagesAssembly, "publisherName");

// For Azure Service Bus Transport (Legacy) version 9.x
var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
var topology = transport.UseEndpointOrientedTopology();

topology.RegisterPublisher(typeof(MyMessage), "publisherName");
// OR
var messagesAssembly = Assembly.LoadFrom("path/to/assembly/containing/messages");
topology.RegisterPublisher(messagesAssembly, "publisherName");

// For Azure Service Bus Transport (Legacy) version 8.x
var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
var topology = transport.UseEndpointOrientedTopology();

topology.RegisterPublisher(typeof(MyMessage), "publisherName");
// OR
var messagesAssembly = Assembly.LoadFrom("path/to/assembly/containing/messages");
topology.RegisterPublisher(messagesAssembly, "publisherName");

// For Azure Service Bus Transport (Legacy) version 7.x
var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
var topology = transport.UseEndpointOrientedTopology();

topology.RegisterPublisher(typeof(MyMessage), "publisherName");
// OR
var messagesAssembly = Assembly.LoadFrom("path/to/assembly/containing/messages");
topology.RegisterPublisher(messagesAssembly, "publisherName");
```

## Sanitization

Azure Service Bus entities have path and naming rules that limit the allowed characters and maximum length.  NServiceBus derives entity names from endpoint and event names; these derived names may fail Azure Service Bus validation. Sanitization allows to modify entity names to meet the namning rules of the broker.

In Versions 6 and below sanitization was performed by default and the MD5 algorithm was used to truncate entity names. In Versions 7 and above, the sanitization has to configured explicitly.

In order to maintain backward compatibility, register a custom sanitization strategy.

In version 6.4.0 `NamingConventions` class was introduced to customize sanitization. The class is obsoleted. Instead, implement a custom sanitization strategy.


## New Configuration API

In Versions 6 and below the transport was configured using the `AzureServiceBusQueueConfig`configuration section. This section has been removed in favor of a more granular code based configuration API.

The new API is accessible through the `.UseTransport<AzureServiceBusTransport>()` extension point.


### Configuring a connection string

Setting a connection string is still supported using the `ConnectionString` extension method:

```csharp
var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
transport.ConnectionString("Endpoint=sb://[NAMESPACE].servicebus.windows.net/;SharedAccessKeyName=[KEYNAME];SharedAccessKey=[KEY]");
```


### Default value changes

The default values for the following settings have changed:

 * `BatchSize`, which had a default value of 1000, is replaced by `PrefetchCount` with a default value of 200.
 * `MaxDeliveryCount` is set to number of immediate retries + 1. For system queues the value has changed from 6 to 10.

### Setting Entity Property Values

The other configuration properties found on `AzureServiceBusQueueConfig` were related to the configuration of Azure Service Bus entities. Even though the name implied differently, these settings were not only applied to queues, but also to topics and subscriptions.

In the new configuration API the settings for queues, topics and subscriptions can be configured individually using the `Queues()`, `Topics()` and `Subscriptions()` extension points.

For example the lock duration on a queue can be configured using the `LockDuration` setting:

```csharp
// For Azure Service Bus Transport (Legacy) version 10.x
var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
var queues = transport.Queues();
queues.LockDuration(TimeSpan.FromMinutes(1));

// For Azure Service Bus Transport (Legacy) version 9.x
var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
var queues = transport.Queues();
queues.LockDuration(TimeSpan.FromMinutes(1));

// For Azure Service Bus Transport (Legacy) version 8.x
var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
var queues = transport.Queues();
queues.LockDuration(TimeSpan.FromMinutes(1));

// For Azure Service Bus Transport (Legacy) version 7.x
var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
var queues = transport.Queues();
queues.LockDuration(TimeSpan.FromMinutes(1));
```

and the size of the topics can be configured using the `MaxSizeInMegabytes` setting:

```csharp
// For Azure Service Bus Transport (Legacy) version 10.x
var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
var topics = transport.Topics();
topics.MaxSizeInMegabytes(SizeInMegabytes.Size5120);

// For Azure Service Bus Transport (Legacy) version 9.x
var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
var topics = transport.Topics();
topics.MaxSizeInMegabytes(SizeInMegabytes.Size5120);

// For Azure Service Bus Transport (Legacy) version 8.x
var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
var topics = transport.Topics();
topics.MaxSizeInMegabytes(SizeInMegabytes.Size5120);

// For Azure Service Bus Transport (Legacy) version 7.x
var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
var topics = transport.Topics();
topics.MaxSizeInMegabytes(SizeInMegabytes.Size5120);
```


## Securing Credentials

include: asb-credential-warning

In order to enhance security and to avoid sharing sensitive information enable the `UseNamespaceNameInsteadOfConnectionString` feature using the following steps:

 * Upgrade all endpoints to Version 7 or above. Previous versions of the transport aren't able to understand namespace names instead of connection strings.
 * After the above has been done and all endpoints have been deployed switch on the `UseNamespaceNameInsteadOfConnectionString` feature and re-deploy.


## BrokeredMessage conventions

In versions 6 and below, `BrokeredMessage` conventions were specified using `BrokeredMessageBodyConversion` class. In versions 7 and above, it has been replaced by configuration API.
