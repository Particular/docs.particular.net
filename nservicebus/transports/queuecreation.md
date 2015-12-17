---
title: Declaring and creating queues
summary: Explains how to declare additional queues and create them
tags:
 - Queue
 - QueueBindings
 - Queue creation
---

This page describes the declaration and creation process of queues. Usually only transports need to deal with queue declaration and creation.

NServiceBus queue creation process is pluggable. The process consists of two phases:

* Declaring queues which need to be created
* Creating the declared queues.


## Declaration

Queues should be declared during the Setup phase of a [Feature](/nservicebus/pipeline/features.md).

A built-in example is the audit feature which needs the audit queue. During start-up NServiceBus ensures the declared queues are present and aborts the start-up procedure if they are not (with an exception of MSMQ remote queues which are optional).

snippet:queuebindings


## Creation

Queues get created during [installation](/nservicebus/operations/installers.md) time only.

Transports need to implement a custom queue creator.

In NServiceBus v6 it is the responsibility of the queue creator to either sequentially or concurrently create the queues provided in the queue bindings for the specified identity.

In NServiceBus v5 and lower the queue creation process is always executed sequentially.

Here a sample of a sequential queue creator

snippet:SequentialCustomQueueCreator

Here a sample of a concurrent queue creator

snippet:ConcurrentCustomQueueCreator

The custom queue creator needs to be registered.

snippet:RegisteringTheQueueCreator