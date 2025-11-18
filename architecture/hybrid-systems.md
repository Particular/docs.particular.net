---
title: Hybrid systems
summary: Guidance for hybrid cloud/on-prem: when to use shared vs separate brokers, and how a Messaging Bridge (e.g., NServiceBus Bridge) links the two.
reviewed: 2025-06-26
callsToAction: ['solution-architect', 'poc-help']
---

Hybrid systems have components running both in the cloud and on-premises. This might be a temporary situation in a lift and shift strategy, or it can be a permanent state due to cost, legal requirements, risk management, or other reasons. Hybrid systems can become tricky to implement when the on-premises part of the system relies on an existing message broker. Some broker technologies might also not be compatible with modern cloud infrastructure, such as MSMQ.

## Shared broker

In the shared broker approach, both systems use a single message broker which can either be located on-premises or in the cloud.

### Cloud

Cloud-hosted message queueing systems can be configured for access from anywhere. While cloud-hosted components benefit from lower network latency and better network reliability, on-premises components can access the same broker.

### On-premises

When it is expected that the message queuing system should remain on-premises and the broker client SDK is cloud compatible, the cloud-hosted messaging clients might be connected to the on-premises broker using cloud vendor-specific VPN services.

## Multiple brokers

Distributed systems might have to work with multiple brokers. In such scenarios, explicit mapping between the actively used messaging systems is required.

### Messaging Bridge

The [Messaging Bridge pattern](https://www.enterpriseintegrationpatterns.com/patterns/messaging/MessagingBridge.html) can be used to connect two separate messaging systems. The bridge is a dedicated component that can generically or selectively map messages across messaging systems.

The [NServiceBus Message Bridge](/nservicebus/bridge/) provides a customizable implementation of the Messaging Bridge pattern which can connect on-premises and cloud components transparently:

![](/samples/bridge/azure-service-bus-msmq-bridge/msmq-to-azure-service-bus-transport-bridge-sample.png)

[**Sample: Try the NServiceBus.MessageBridge sample →**](/samples/bridge/simple/)
