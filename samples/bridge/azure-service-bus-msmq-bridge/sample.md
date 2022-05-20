---
title: MSMQ to Azure Service Bus Transport Bridging
reviewed: 2022-05-19
component: Bridge
related:
- transports/azure-service-bus
- transports/msmq
- nservicebus/bridge
redirects:
- samples/bridge/azure-service-bus-msmq-bridge
---

Endpoints running on different transports cannot exchange messages and require additional integration work.

Common examples include:

* A hybrid solution that spans across endpoints deployed on-premises and in a cloud environment.
* Departments within organization integrating their systems that use different messaging technologies for historical reasons.

Traditionally, such integrations would require native messaging or relaying. Bridging is an alternative, allowing endpoints to communicate over different transports without a need to get into low-level messaging technology code. With time, when endpoints can standardize on a single transport, bridging can be removed with a minimal impact on the entire system.

## Prerequisites

include: asb-connectionstring-xplat

## Code walk-through

This sample shows an integration between two endpoints running on two different transports, `MsmqEndpoint` endpoint running on MSMQ and `AsbEndpoint` running on Azure Service Bus.

Covered scenarios are:

* Sending commands from Azure Service Bus endpoint to MSMQ endpoint.
* Publishing events from MSMQ endpoint and subscribing to those events from Azure Service Bus endpoint.
* Publishing events from Azure Service Bus and subscribing to those events from the MSMQ endpoint.

### Bridging

Endpoints are bridged using [NServiceBus.Transport.Bridge](/nservicebus/bridge/), which is implemented as a standalone process that runs side-by-side with the bridged endpoints, `MsmqEndpoint` and `AsbEndpoint`.  These endpoints are not aware that there is a bridge involved in the sending and receiving of messages.  They simply send/publish messages as if the other endpoints are on the same transport.  All of the configuration to bridge different transports is handled in the bridge endpoint.

Bridge process is connecting between MSMQ and Azure ServiceBus transports and provide any configuration settings required by any of the transports. For Azure Service Bus that would be a mandatory connection string and topology.

#### Azure Service Bus bridge endpoint configuration

The Azure Service Bus bridge endpoint is configured by using the name of the actual Azure Service Bus endpoint where the message needs to be routed to:

snippet: create-asb-endpoint-of-bridge

To subscribe to an event published by MSMQ endpoint, Azure Service Bus endpoint must register the publishing endpoint using bridge endpoint method:

snippet: asb-subscribe-to-event-via-bridge

When the bridge endpoint has been created and the publisher has been registered, the endpoint is added to the transport and then the transport is added to the bridge configuration.

snippet: asb-bridge-configuration

#### MSMQ bridge endpoint configuration

MSMQ bridge endpoint is configured by using the name of the actual MSMQ endpoint where the message needs to be routed to:

NOTE: The `QueueAddress` parameter is needed to create an MSMQ bridge endpoint when the actual MSMQ endpoint and the bridge are running on separate servers 

snippet: create-msmq-endpoint-of-bridge

To subscribe to an event published by Azure Service Bus endpoint, MSMQ endpoint must register the publishing endpoint using bridge endpoint method:

snippet: msmq-subscribe-to-event-via-bridge

When the bridge endpoint has been created and the publisher has been registered, the endpoint is added to the transport and then the transport is added to the bridge configuration.

snippet: msmq-bridge-configuration

 



