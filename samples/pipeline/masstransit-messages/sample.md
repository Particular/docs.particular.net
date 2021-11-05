---
title: Consuming messages from MassTransit
summary: Use the NServiceBus pipeline to consume messages sent by MassTransit.
reviewed: 2021-11-04
component: Core
related:
 - nservicebus/pipeline
---

Intro text

downloadbutton

## Prerequisites

This sample requires a local instance of RabbitMQ.

## MassTransit publisher

snippet: MassTransitPublish

snippet: MassTransitEvent

snippet: MassTransitConsumer

## NServiceBus subscriber

snippet: Serializer

snippet: Transport

snippet: Conventions

snippet: RegisterBehavior

snippet: NSBMessageHandler