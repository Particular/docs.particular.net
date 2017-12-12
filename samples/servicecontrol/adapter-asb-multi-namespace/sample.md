---
title: Monitor Azure Service Bus endpoints with the ServiceControl adapter
summary: Centralized monitoring of Azure Service Bus endpoints with the ServiceControl adapter
component: SCTransportAdapter
reviewed: 2017-12-12
related:
 - servicecontrol
 - servicecontrol/transport-adapter
 - servicecontrol/plugins
 - transports/azure-service-bus
---


This sample shows how to configure ServiceControl to monitor endpoints and retry messages when using the advanced features of the Azure Service Bus transport [not natively supported by ServiceControl](/servicecontrol/transport-adapter/incompatible-features.md#azure-service-bus).

The following diagram shows the topology of the solution:

```mermaid
graph RL

  subgraph Namespace 2
    shipping["fa:fa-truck Shipping"]
  end

  subgraph Namespace 1
    sales["fa:fa-money Sales"]
  end

  subgraph Namespace 3
    sc["fa:fa-wrench Service Control"]
  end

  adapter{"fa:fa-exchange Adapter"}

  sales ==> adapter
  adapter .-> sales
  shipping==>adapter
  adapter .-> shipping
  adapter==>sc
  sc .-> adapter
```

Notice that `Sales` and `Shipping` are in two different namespaces. This is done by using [cross-namespace routing](/transports/azure-service-bus/multiple-namespaces-support.md#cross-namespace-routing). The other important thing to note is that ServiceControl is in a different namespace from the other endpoints, which means that it can't natively communicate with them, which is why this sample shows how to create an adapter to bridge between everything.

The adapter also deals with other advanced features of the Azure Service Bus transport like [secure connection strings](/transports/azure-service-bus/securing-connection-strings.md) and [customized brokered message creation](/transports/azure-service-bus/brokered-message-creation.md).

## Prerequisistes

include: asb-connectionstring

 1. An environment variable named `AzureServiceBus.ConnectionString.1` with the connection string for the Azure Service Bus namespace to be used by Sales endpoint.
 1. An environment variable named `AzureServiceBus.ConnectionString.2` with the connection string for the Azure Service Bus namespace to be used by Shipping endpoint.
 1. An environment variable named `AzureServiceBus.ConnectionString.SC` with the connection string for the Azure Service Bus namespace to be used by ServiceControl and the adapter.
 1. [Install ServiceControl](/servicecontrol/installation.md).
 1. Using [ServiceControl Management](/servicecontrol/license.md#servicecontrol-management-app) tool, set up ServiceControl to monitor endpoints using Azure Service Bus transport:
	 
   * Add a new ServiceControl instance: 
   * Use `Particular.ServiceControl.ASB` as the instance name (ensure there is no other instance of SC running with the same name).
   * Use connection string supplied with the `AzureServiceBus.ConnectionString.SC` environment variable.
   
NOTE: If other ServiceControl instances have been running on this machine, it's necessary to specify a non-default port number for API. [Adjust ServicePulse settings](/servicepulse/host-config.md#changing-the-servicecontrol-url) accordingly to point to this location.
 
 1. Ensure the `ServiceControl` process is running before running the sample.
 1. [Install ServicePulse](/servicepulse/installation.md)

include: adapter-running-project


## Code walk-through 

The code base consists of four projects.


### Shared

The Shared project contains the message contracts.


### Sales and Shipping

The Sales and Shipping projects contain endpoints that simulate the execution of a business process. The process consists of two messages: `ShipOrder` command sent by Sales and `OrderShipped` reply sent by Shipping.

The Sales and Shipping endpoints include a message processing failure simulation mode (toggled by pressing `f`) which can be used to generate failed messages for demonstrating message retry functionality.

The Shipping endpoint has the Heartbeats plugin installed to enable uptime monitoring via ServicePulse.

Both endpoints are configured to use:

 * [Secure connection strings](/transports/azure-service-bus/securing-connection-strings.md).
 * [Customized brokered message creation](/transports/azure-service-bus/brokered-message-creation.md) using `Stream`.
 * Different namespaces with [cross-namespace routing](/transports/azure-service-bus/multiple-namespaces-support.md#cross-namespace-routing) enabled.
 * [Namespace hierary](/transports/azure-service-bus/namespace-hierarchy.md) to prefix all entities with `scadapter/`.

snippet: featuresunsuportedbysc


### Adapter

The Adapter project hosts the `ServiceControl.TransportAdapter`. The adapter has two sides:

 * endpoint facing
 * ServiceControl facing

In this sample both use Azure Service Bus transport:

snippet: AdapterTransport

The following code configures the adapter to match advanced transport features enabled on the endpoints:

 * [Secure connection strings](/transports/azure-service-bus/securing-connection-strings.md).
 * [Customized brokered message creation](/transports/azure-service-bus/brokered-message-creation.md) using `Stream`.
 * [Multiple namespace](/transports/azure-service-bus/multiple-namespaces-support.md#round-robin-namespace-partitioning).
 * [Namespace hierary](/transports/azure-service-bus/namespace-hierarchy.md)

snippet: EndpointSideConfig

While the following code configures the adapter to communicate with ServiceControl:

snippet: SCSideConfig

Since ServiceControl has been installed under a non-default instance name (`Particular.ServiceControl.ASB`) the control queue name needs to be overridden in the adapter configuration:

snippet: ControlQueueOverride

Shipping and Sales use different namespaces, therefore the adapter has to be configured to properly route retried messages:

snippet: UseNamespaceHeader

The destination address consists of the queue name and the namespace alias which is included in the failed messages:

snippet: NamespaceAlias
