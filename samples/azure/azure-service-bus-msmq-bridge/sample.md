---
title: MSMQ to Azure Service Bus Transport Bridge
reviewed: 2017-10-17
component: ASB
related:
- transports/azure-service-bus
- nservicebus/bridge
---


Endpoints running on different transports cannot exchange messages and require additional integration work.

Common examples include:

 * A hybrid solution that spans across endpoints deployed on-premises and in a cloud environment.
 * Departments within organization integrating their systems that use different messaging technologies for historical reasons.

Traditionally, such integrations would require native messaging or relaying. Bridging is an alternative, allowing endpoints to communicate over different transports without a need to get into low-level messaging technology code. With time, when endpoints can standardize on a single transport, bridging can be removed with a minimal impact on the entire system.


## Prerequisites

include: asb-connectionstring

include: asb-transport


## Code walk-through

This sample shows an integration between two endpoints running on two different transports, `MsmqEndpoint` endpoint running on MSMQ and `AsbEndpoint` running on Azure Service Bus.

Covered scenarios are:

 * Sending commands from Azure Service Bus endpoint to MSMQ endpoint.
 * Publishing events from MSMQ endpoint and subscribing to those events from Azure Service Bus endpoint.


### Bridging

Endpoints are bridged using [Transport Bridge](/nservicebus/bridge/). `Bridge` project is implemented as a standalone process that runs side-by-side with the bridged endpoints, `MsmqEndpoint` and `AsbEndpoint`.


#### Azure Service Bus endpoint configuration

Azure Service Bus endpoint is bridged via `Bridge-ASB` queue:

snippet: connect-asb-side-of-bridge

![Azure Service Bus topology][asb-topology]

Command routing to MSMQ endpoint is specified using bridge extension method:

snippet: route-command-via-bridge

NOTE: to access bridge extension method, project has to reference `NServiceBus.Bridge.Connector` NuGet package.

To subscribe to an event published by MSMQ endpoint, Azure Service Bus endpoint must register publishing endpoint using bridge extension method: 

snippet: subscribe-to-event-via-bridge


#### MSMQ endpoint configuration

MSMQ endpoint is bridged via `Bridge-MSMQ` queue:

![MSMQ topology][msmq-topology]


#### Bridge configuration

Bridge process is connecting between MSMQ and Azure ServiceBus transports and provide any configuration settings required by any of the transports. For Azure Service Bus that would be a mandatory connection string and topology.

snippet: bridge-general-configuration

A bridge is created, started, and should be executed as long as bridging is required.

NOTE: This sample use a simple `InMemorySubscriptionStorage`. In production `SqlSubscriptionStorage` (included in the Bridge package) or custom persistent subscription storage should be used to prevent message loss.  

snippet: bridge-execution

#### Forwarding interception

In scenarios where forwarding operation requires customization, Bridge provides an option to intercept forwarding operation. For example, Azure Service Bus transport requires any non Azure Service Bus [transactional scope to be suppressed](/transports/azure-service-bus/understanding-transactions-and-delivery-guarantees.md). 

snippet: suppress-transaction-scope-for-asb

[asb-topology]: asb-topology.png "Azure Service Bus topology"
[msmq-topology]: msmq-topology.png "MSMQ topology"
