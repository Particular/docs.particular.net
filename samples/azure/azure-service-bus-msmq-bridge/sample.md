---
title: MSMQ to Azure Service Bus Transport Bridge
reviewed: 2017-10-17
component: ASB
related:
- transports/azure-service-bus
- nservicebus/bridge
---


## Prerequisites

include: asb-connectionstring


include: asb-transport


## Code walk-through

This sample shows an integration between two endpoints running on two different transports, `MsmqEndpoint` endpoint running on MSMQ and `AsbEndpoint` running on Azure Service Bus.

Covered scenarios are:

 * Sending commands from MSMQ endpoint to Azure Service Bus endpoint.
 * Publishing events from MSMQ endpoint and subscribing to those events from Azure Service Bus endpoint.

### Bridging

Endpoints are bridged using [Transport Brindge](/nservicebus/bridge/). `Bridge` project is implemented as a standalone process that runs side-by-side with the bridged endpoints, `MsmqEndpoint` and `AsbEndpoint`.

#### Bridge configuration

#### MSMQ endpoint configuration

#### Azure Service Bus endpoint configuration