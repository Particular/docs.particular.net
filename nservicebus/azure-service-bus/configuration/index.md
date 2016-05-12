---
title: Azure Service Bus Transport Configuration
summary: Configuring Azure Service Bus as transport
tags:
- Azure
- Cloud
- Configuration
---

## Getting Started

In order to setup a basic endpoint using the Azure Service Bus transport the following configuration settings need to be provided.

snippet:AzureServiceBusTransportGettingStarted

 * `UseTransport<AzureServiceBusTransport>()`: Enables the Azure Service Bus transport.
 * `UseTopology<ForwardingTopology>()`: Defines what the underlying layout of Azure Service Bus messaging entities looks like.
 * `ConnectionString()`: Specifies the connection string of the Azure Service Bus namespace to be used by the transport.
 * `UsePersistence<InMemoryPersistence>()`: The Azure Service Bus transport has no dependency on persistence, so the in memory one will be sufficient. If there is a need to use sagas, one of the other persister implementations needs to be provided.

## Full Configuration API

In Versions 7 and above, the Azure Service Bus transport has an extensive, code-only, configuration API. This API can be accessed from the `UseTransport<AzureServiceBusTransport>()` extension method. A full listing of the API can be found on [the configuration API page](/nservicebus/azure-service-bus/configuration/full.md)

The configuration API provides fine grained access to the behavior of different layers in the transport.

 * `UseTopology<T>()` controls how NServiceBus will lay out entities in the Azure Service Bus namespace.

In order to control the creation of these entities, the API provides direct access to the underlying configuration description objects through the following extension methods.

 * [`Queues()`](/nservicebus/azure-service-bus/configuration/full.md#controlling-entities-queues)
 * [`Topics()`](/nservicebus/azure-service-bus/configuration/full.md#controlling-entities-topics)
 * [`Subscriptions()`](/nservicebus/azure-service-bus/configuration/full.md#controlling-entities-subscriptions)

Connectivity settings are available at both the factory as well as receive/send client level.

 * [`MessagingFactories()`](/nservicebus/azure-service-bus/configuration/full.md#controlling-connectivity-messaging-factories)
 * [`MessageReceivers()`](/nservicebus/azure-service-bus/configuration/full.md#controlling-connectivity-message-receivers)
 * [`MessageSenders()`](/nservicebus/azure-service-bus/configuration/full.md#controlling-connectivity-message-senders)

There have historically been regular changes to the physical addressing logic, from naming conventions to assumptions and validations in both NServiceBus as well as Azure Service Bus itself. The following extension methods provide access to all of the aspects related to this addressing logic, so that the transport can be adapted to any future changes in this respect.

 * [`NamespacePartitioning()`](/nservicebus/azure-service-bus/configuration/full.md#physical-addressing-logic-namespace-partitioning)
 * [`Composition()`](/nservicebus/azure-service-bus/configuration/full.md#physical-addressing-logic-composition)
 * [`Validation()`](/nservicebus/azure-service-bus/configuration/full.md#physical-addressing-logic-validation)
 * [`Sanitization()`](/nservicebus/azure-service-bus/configuration/full.md#physical-addressing-logic-sanitization)
 * [`Individualization()`](/nservicebus/azure-service-bus/configuration/full.md#physical-addressing-logic-individiualization)


## AzureServiceBusQueueConfig

In versions 6 and below, configuration values are provided using the `AzureServiceBusQueueConfig` configuration section, refer to the [AzureServiceBusQueueConfig configuration page](/nservicebus/azure-service-bus/configuration/azureservicebusqueueconfig.md) for more details.