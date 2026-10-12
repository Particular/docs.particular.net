---
title: Azure Service Bus Transport Topology
component: ASBS
summary: How Azure Service Bus transport routes events using topics, subscriptions, and forwarding, including topology limits and message contract evolution.
related:
 - transports/azure-service-bus/configuration
reviewed: 2026-10-12
---

Messaging topology is a specific arrangement of messaging entities, such as queues, topics, subscriptions, and rules.

Azure Service Bus transport operates on a topology created on the broker. The topology handles exchanging messages between endpoints, by creating and configuring Azure Service Bus entities.

partial: description