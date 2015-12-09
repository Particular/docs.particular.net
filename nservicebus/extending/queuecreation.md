---
title: Declaring and creating queues
summary: Explains how to declare additional queues and create them
tags:
 - Queue
 - QueueBindings
 - Queue creation
---

NServiceBus queue creation process is pluggable. The process consists of two phases:

* Declaring queues which need to be created
* Creating the declared queues.

This page describes the declaration and creation process of queues. Usually only transports need to deal with queue declaration and creation.

## Declaration

Queues should be declared during the Setup phase of a Feature. Read more how to write a Feature on the [Feature documentation page](/nservicebus/pipeline/feature.md).

A built-in example is the audit feature which needs the audit queue. During start-up NServiceBus ensures the declared queues are present and aborts the start-up procedure if they are not (with an exception of MSMQ remote queues which are optional).

snippet:queuebindings

## Creation

Queues get created during installation time only. More information about Installers can be found on the [Installer documentation page](nservicebus/operations/installers.md).

Transports need to implement a custom queue creator. It is the responsibility of the queue creator to either sequentially or concurrently create the queues provided in the queue bindings for the specified identity.

snippet:CustomQueueCreator

The factory for creating the queue creator needs to be provided in the transport definition.

snippet:TransportDefinitionForQueueCreator
