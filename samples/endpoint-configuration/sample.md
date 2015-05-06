---
title: Endpoint configuration choices
summary: Walks through the standard configuration options for hosting an endpoint.
related:
- samples/step-by-step
- nservicebus/hosting/assembly-scanning
---

Note: This sample uses the same approach as the [NServiceBus as a Windows Service](/samples/hosting/windows-service/) for a streamlined Windows Service debugging experience.

## Code walk-through

This samples walks through the most common choices you will need to make when creating your first endpoint. It will also show the configuration APIs needed to implement those choices. While this sample uses a Windows Service for hosting the same decisions will need to be made not matter the [Hosting choice](/nservicebus/hosting). 

## Configure an [Error](/nservicebus/errors) queue

When a message fails processing it will be forwarded here.

<!-- import error -->

Note that this approach uses the [IProvideConfiguration](/nservicebus/hosting/custom-configuration-providers.md) approach to programmatically override the error queue.

## Configure an [Audit](/nservicebus/operations/auditing.md) queue

All messages received by an endpoint will be forwarded to the audit queue.

<!-- import audit -->

## Select and configure [Logging](/nservicebus/logging)

Log4net is being used to route log events to the Console.

<!-- import logging -->
 
## Create the root configuration instance

<!-- import create-config -->

## Define the Endpoint Name

<!-- import endpoint-name -->

## Select and configure a [Container](/nservicebus/containers)

Autofac is being used with a customized container instance being passed into NServiceBus.

<!-- import container -->

## Select and configure [Serialization](/nservicebus/serialization)

<!-- import serialization -->

## Select and configure a [Transport](/nservicebus/transports)

<!-- import transport -->

## Enable [Sagas](/nservicebus/sagas)

<!-- import sagas -->

## Select and configure [Persistence](/nservicebus/persistence)

<!-- import persistence -->

## Start the Bus

Enable installers and start the bus.

<!-- import start-bus -->

## Shut down the bus

The bus implements `IDisposable` and should be shut down when the process is shut down.

<!-- import stop-bus -->

## Handling [Critical Errors](/nservicebus/hosting/critical-errors.md)
Since this sample is configured to run as a windows service, the action defined when a critical error occurs is to shut down the process. 

<!-- import critical-errors -->
