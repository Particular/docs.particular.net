---
title: Hybrid systems
summary: Systems deployed both in the cloud and on-premises
reviewed: 2023-07-18
---

Hybrid systems contain of components of the same system running on the cloud and components running on-premises. This might be a temporary situation in a lift and shift strategy, or it can be permanent state for various reasons (e.g. risk, legal, costs, etc.). Hybrid scenarios can become tricky to implement when the on-premises part of the system relies on an existing message broker. Some broker technologies might also not be compatible with modern cloud infrastructure (e.g. MSMQ).

## Shared Broker

In the shared broker approach, both systems use a single message broker which can either be located on-premises or in the cloud.

### On-premises

When it is expected that the message queuing system should remain on-premises and the broker client SDK is cloud compatible, the cloud hosted messaging clients might be connected to the on-premises broker using cloud vendor specific VPN services.

### Cloud

Cloud hosted message queueing systems can be configured to be accessed from anywhere in the world. While cloud hosted services will benefit from lower network latency and better network reliability, on-premises systems can access the same broker.


## Messaging Bridge

The [Messaging Bridge pattern](https://www.enterpriseintegrationpatterns.com/patterns/messaging/MessagingBridge.html) can be used to connect two separate messaging systems.

The [NServiceBus Message Bridge](/nservicebus/bridge/) provides a customizable implementation of the messaging bridge pattern which can connect on-premises and cloud services transparantly:

![](/samples/bridge/azure-service-bus-msmq-bridge/msmq-to-azure-service-bus-transport-bridge-sample.png)

[**Sample: Try the NServiceBus.MessageBridge sample →**](/samples/bridge/simple/)