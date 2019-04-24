---
title: Transitioning to the Azure Service Bus Transport
summary: Demonstrates the required changes to switch your POC from the Learning Transport to the Azure Service Bus Transport
reviewed: 2019-04-24
component: ASBS
tags:
 - Azure
 - Transport
related:
 - transports/azure-service-bus
 - samples/azure-service-bus-netstandard/send-reply
---

The Learning transport is not a production-ready transport, but is intended for learning the NServiceBus API and creating demos/proof-of-concepts. A number of changes to endpoint configuration are required to transition an endpoint from the Learning transport to the Azure Service Bus transport.


## Install the NuGet Package

The [NServiceBus.Transport.AzureServiceBus](https://www.nuget.org/packages/NServiceBus.Transport.AzureServiceBus/) must be installed. This can be accomplished using the [NuGet Package Manager UI](https://docs.microsoft.com/en-us/nuget/tools/package-manager-ui) or run this command from the [NuGet Package Manager Console](https://docs.microsoft.com/en-us/nuget/tools/package-manager-console):

```
   Install-Package NServiceBus.Transport.AzureServiceBus
```


## Change the Endpoint Configuration

To use the Azure Service Bus Transport first update the `UseTransport` call:

```c#
-   endpointConfiguration.UseTransport<LearningTransport>();
+   endpointConfiguration.UseTransport<AzureServiceBusTransport>();
```

### Set a Connection String

The Azure Service Bus Transport requires a connection string be specified:

snippet: azure-service-bus-for-dotnet-standard


### Enable Installers

The endpoint must enable installers to allow the Azure Service Bus transport to create the necessary [queues, topics, and subscriptions](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-queues-topics-subscriptions) in the namespace for your endpoint.

snippet: Installers
