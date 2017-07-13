---
title: Endpoint configuration choices
summary: Shows the standard configuration options for hosting an endpoint.
reviewed: 2016-03-18
component: Core
related:
- samples/step-by-step
- nservicebus/hosting/assembly-scanning
---

Note: This sample uses the same approach as the [NServiceBus as a Windows Service](/samples/hosting/windows-service/) for a streamlined Windows Service debugging experience.


## Code walk-through

This sample walks through the most common choices required when creating a first endpoint. It will also show the configuration APIs needed to implement those choices.


## Hosting

This sample uses a dual runnable console and Windows Service for hosting. More details on this approach can be seen in [Windows Service Hosting](/nservicebus/hosting/windows-service.md), there is also [a more detailed sample](/samples/hosting/windows-service/) of this approach. See also [Hosting options](/nservicebus/hosting).


## Configure an [Recoverability](/nservicebus/recoverability/)

When a message fails processing it will be forwarded here.

snippet: error

partial: error


## Configure an [Audit](/nservicebus/operations/auditing.md) queue

All messages received by an endpoint will be forwarded to the audit queue.

snippet: audit


## Select and configure [Logging](/nservicebus/logging)

In this sample [Log4net](/nservicebus/logging/log4net.md) is being used to route log events to the Console.

snippet: logging


## Create the root configuration instance

And also define the endpoint name.

partial: endpointname

snippet: create-config


## Select and configure a [Container](/nservicebus/containers)

[Autofac](/nservicebus/containers/autofac.md) is being used with a customized container instance being passed into NServiceBus.

snippet: container


## Select and configure [Serialization](/nservicebus/serialization)

This sample uses the [JSON](/nservicebus/serialization/json.md) serializer.

snippet: serialization


## Select and configure a [Transport](/transports)

partial: transport

snippet: transport


## Select and configure [Persistence](/persistence)

partial: persistence

snippet: persistence


## Start the [Endpoint](/nservicebus/endpoints/)

Enable [installers](/nservicebus/operations/installers.md) and start the endpoint.

snippet: start-bus


## Shut down the [Endpoint](/nservicebus/endpoints/)

The bus implements `IDisposable` and should be shut down when the process is shut down.

snippet: stop-endpoint


## Handling [Critical Errors](/nservicebus/hosting/critical-errors.md)

Since this sample is configured to run as a [windows service](/nservicebus/hosting/windows-service.md), the action defined when a critical error occurs is to shut down the process.

snippet: critical-errors
