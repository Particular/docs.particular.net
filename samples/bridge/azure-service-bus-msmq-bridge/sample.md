---
title: Bridge messages between endpoints using MSMQ and Azure Service Bus
summary: A sample showing how to send messages between endpoints using different transports using NServiceBus Messaging Bridge
reviewed: 2025-08-08
component: Bridge
related:
- transports/azure-service-bus
- transports/msmq
- nservicebus/bridge
redirects:
- samples/bridge/azure-service-bus-msmq-bridge
---

Endpoints running on different transports cannot exchange messages directly and must use the `NServiceBus.MessagingBridge` component to communicate.

Common examples include:

- A hybrid solution that spans across endpoints deployed on-premises and in a cloud environment.
- Departments within an organization integrating their systems that use different messaging technologies for historical reasons.

Traditionally, these integrations would require native messaging or relaying. Bridging is an alternative that allows endpoints to communicate over different transports without requiring knowledge of low-level messaging technology.

## Prerequisites

include: asb-connectionstring-xplat

## Code walk-through

This sample shows an integration between two endpoints running on two different transports: `MsmqEndpoint` running on MSMQ and `AsbEndpoint` running on Azure Service Bus.

The scenarios covered by the sample include:

- Sending commands from an Azure Service Bus endpoint to an MSMQ endpoint.
- Publishing events from an MSMQ endpoint and subscribing to those events from an Azure Service Bus endpoint.
- Publishing events from Azure Service Bus and subscribing to those events from an MSMQ endpoint.

![msmq to azure service bus messaging bridge sample](msmq-to-azure-service-bus-transport-bridge-sample.png 'width=500')

### Bridging

Endpoints are bridged using [NServiceBus.MessagingBridge](/nservicebus/bridge/), which is a standalone process that runs side-by-side with the bridged endpoints, `MsmqEndpoint` and `AsbEndpoint`. These endpoints are not aware that there is a bridge involved in the sending and receiving of messages. They send/publish messages as if the entire system uses the same transport. All configuration for bridging different transports is handled in the bridge code.

The bridge connects the MSMQ and Azure Service Bus endpoints, providing the configuration settings required by each transport. For example, Azure Service Bus requires a connection string and the topology to be set.

#### Azure Service Bus bridge endpoint configuration

The Azure Service Bus bridge endpoint is configured by using the name of the actual Azure Service Bus endpoint where the message needs to be routed to:

snippet: create-asb-endpoint-of-bridge

To subscribe to an event published by the MSMQ endpoint, the Azure Service Bus endpoint must register the publishing endpoint:

snippet: asb-subscribe-to-event-via-bridge

When the bridge endpoint has been created and the publisher has been registered, the endpoint is added to the transport, and then the transport is added to the bridge configuration.

snippet: asb-bridge-configuration

#### MSMQ bridge endpoint configuration

The MSMQ bridge endpoint is configured by using the name of the actual MSMQ endpoint where the message needs to be routed to:

> [!NOTE]
> The `QueueAddress` parameter is needed to create an MSMQ bridge endpoint when the actual MSMQ endpoint and the bridge are running on separate servers.

snippet: create-msmq-endpoint-of-bridge

To subscribe to an event published by the Azure Service Bus endpoint, the MSMQ endpoint must register the publishing endpoint:

snippet: msmq-subscribe-to-event-via-bridge

When the bridge endpoint has been created and the publisher has been registered, the endpoint is added to the transport, and then the transport is added to the bridge configuration.

snippet: msmq-bridge-configuration
