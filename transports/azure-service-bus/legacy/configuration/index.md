---
title: Configuration
component: ASB
versions: '[7,)'
reviewed: 2021-01-04
redirects:
 - nservicebus/azure-service-bus/configuration
 - transports/azure-service-bus/configuration
---

include: legacy-asb-warning

## Common settings

In order to setup a basic endpoint using the Azure Service Bus transport the following configuration settings need to be provided:

snippet: AzureServiceBusTransportGettingStarted

 * `UseTransport<AzureServiceBusTransport>()`: Enables the Azure Service Bus transport.
 * `UseForwardingTopology()`: Defines the underlying layout of Azure Service Bus messaging entities.
 * `ConnectionString()`: Specifies the connection string of the Azure Service Bus namespace to be used by the transport.
 * `UsePersistence<InMemoryPersistence>()`: The Azure Service Bus transport has no dependency on persistence, so the in memory one will be sufficient. If there is a need to use sagas, one of the other persister implementations needs to be provided.

## Full Configuration API

In Versions 7 and above, the Azure Service Bus transport has an extensive, code-only, configuration API. This API can be accessed from the `UseTransport<AzureServiceBusTransport>()` extension method. A full listing of the API can be found on [the configuration API page](/transports/azure-service-bus/legacy/configuration/full.md)

The configuration API provides fine grained access to the behavior of different layers in the transport.

 * Choosing topology controls how NServiceBus will lay out entities in the Azure Service Bus namespace.

In order to control the creation of these entities, the API provides direct access to the underlying configuration description objects through the following extension methods.

 * [`Queues()`](/transports/azure-service-bus/legacy/configuration/full.md#controlling-entities-queues)
 * [`Topics()`](/transports/azure-service-bus/legacy/configuration/full.md#controlling-entities-topics)
 * [`Subscriptions()`](/transports/azure-service-bus/legacy/configuration/full.md#controlling-entities-subscriptions)

Connectivity settings are available at both the factory as well as receive/send client level.

 * [`MessagingFactories()`](/transports/azure-service-bus/legacy/configuration/full.md#controlling-connectivity-messaging-factories)
 * [`MessageReceivers()`](/transports/azure-service-bus/legacy/configuration/full.md#controlling-connectivity-message-receivers)
 * [`MessageSenders()`](/transports/azure-service-bus/legacy/configuration/full.md#controlling-connectivity-message-senders)

There have historically been regular changes to the physical addressing logic, from naming conventions to assumptions and validations in both NServiceBus as well as Azure Service Bus itself. The following extension methods provide access to all of the aspects related to this addressing logic, so that the transport can be adapted to any future changes in this respect.

 * [`NamespacePartitioning()`](/transports/azure-service-bus/legacy/configuration/full.md#physical-addressing-logic-namespace-partitioning)
 * [`Composition()`](/transports/azure-service-bus/legacy/configuration/full.md#physical-addressing-logic-composition)
 * [`Sanitization()`](/transports/azure-service-bus/legacy/configuration/full.md#physical-addressing-logic-sanitization)
 * [`Individualization()`](/transports/azure-service-bus/legacy/addressing-logic.md#individualization)


## AzureServiceBusQueueConfig

In versions 6 and below, configuration values are provided using the `AzureServiceBusQueueConfig` configuration section, refer to the [AzureServiceBusQueueConfig configuration page](/transports/azure-service-bus/legacy/configuration/azureservicebusqueueconfig.md) for more details.