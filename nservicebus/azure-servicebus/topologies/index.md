---
title: Azure Service Bus Topologies
summary: Azure Service Bus Topologies
tags:
- Azure
- Cloud
redirects:
related:
 - samples/azure/azure-service-bus
reviewed: 2016-04-12
---

Messaging topology is an arrangement of the various messaging entities.
Azure Service Bus transport operates on a topology created on the broker. Messaging entities participating in topology are queues, topics, subscriptions, and rules. Combined in a specific way they form a topology. 

## Version 6 and below

Version 6 and below of the transport has always supported a single default topology out-of-the-box. 

## Version 7 and up

With version 7 of the transport, there are now two topologies:

1. `ForwardingTopology`
1. `EndpointOrientedTopology`

## ForwardingTopology

## EndpointOrientedTopology