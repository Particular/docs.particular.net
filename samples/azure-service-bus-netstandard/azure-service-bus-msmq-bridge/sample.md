---
title: MSMQ to Azure Service Bus Transport Bridging
reviewed: 2021-05-18
component: ASBS
related:
- transports/azure-service-bus
- nservicebus/router
redirects:
- samples/azure/azure-service-bus-msmq-bridge
---

Endpoints running on different transports cannot exchange messages and require additional integration work.

Common examples include:

 * A hybrid solution that spans across endpoints deployed on-premises and in a cloud environment.
 * Departments within organization integrating their systems that use different messaging technologies for historical reasons.

Traditionally, such integrations would require native messaging or relaying. Bridging is an alternative, allowing endpoints to communicate over different transports without a need to get into low-level messaging technology code. With time, when endpoints can standardize on a single transport, bridging can be removed with a minimal impact on the entire system.


## Prerequisites

include: asb-connectionstring-xplat

include: asb-transport


## Code walk-through

This sample shows an integration between two endpoints running on two different transports, `MsmqEndpoint` endpoint running on MSMQ and `AsbEndpoint` running on Azure Service Bus.

Covered scenarios are:

 * Sending commands from Azure Service Bus endpoint to MSMQ endpoint.
 * Publishing events from MSMQ endpoint and subscribing to those events from Azure Service Bus endpoint.


### Bridging

Endpoints are bridged using [NServiceBus.Router](/nservicebus/router/). `Bridge` project is implemented as a standalone process that runs side-by-side with the bridged endpoints, `MsmqEndpoint` and `AsbEndpoint`.


#### Azure Service Bus endpoint configuration

Azure Service Bus endpoint is bridged via `Bridge` queue:

snippet: connect-asb-side-of-bridge

![Azure Service Bus topology][asb-topology]

The routing of commands to the MSMQ endpoint is specified using bridge extension method:

snippet: route-command-via-bridge

NOTE: to access bridge extension method, project has to reference `NServiceBus.Router.Connector` NuGet package for NServiceBus 7 or `NServiceBus.Bridge.Connector` NuGet package for NServiceBus 6.

To subscribe to an event published by MSMQ endpoint, Azure Service Bus endpoint must register publishing endpoint using bridge extension method: 

snippet: subscribe-to-event-via-bridge


#### MSMQ endpoint configuration

MSMQ endpoint is bridged via `Bridge` queue:

![MSMQ topology][msmq-topology]


#### Bridge configuration

Bridge process is connecting between MSMQ and Azure ServiceBus transports and provide any configuration settings required by any of the transports. For Azure Service Bus that would be a mandatory connection string and topology.

snippet: bridge-general-configuration

A bridge is created, started, and should be executed as long as bridging is required.

NOTE: This sample use a simple `InMemorySubscriptionStorage`. In production `SqlSubscriptionStorage` (included in the NServiceBus.Router package) or custom persistent subscription storage should be used to prevent message loss.

snippet: bridge-execution

[asb-topology]: asb-topology.png "Azure Service Bus topology"
[msmq-topology]: msmq-topology.png "MSMQ topology"
