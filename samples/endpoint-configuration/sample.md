---
title: Endpoint configuration choices
summary: Shows the standard configuration options for hosting an endpoint.
reviewed: 2022-09-22
component: Core
related:
- nservicebus/hosting/assembly-scanning
- nservicebus/hosting
---

WARN: This sample is obsolete. We recommend using the [.NET Generic Host sample](https://docs.particular.net/samples/hosting/generic-host/).

## Code walk-through

This sample walks through the most common choices required when creating a first endpoint. It will also show the configuration APIs needed to implement those choices.

## Hosting

This sample uses a dual runnable console and Windows Service for hosting. Details on how to install an endpoint as a Windows Service can be seen in [Windows Service Installation](/nservicebus/hosting/windows-service.md). See also [Hosting options](/nservicebus/hosting).

## Configure [recoverability](/nservicebus/recoverability/)

When a message fails processing it will be forwarded here.

snippet: error

## Configure an [audit](/nservicebus/operations/auditing.md) queue

All messages received by an endpoint will be forwarded to the audit queue.

snippet: audit

## Select and configure [logging](/nservicebus/logging)

In this sample [Log4net](/nservicebus/logging/log4net.md) is being used to route log events to the console.

snippet: logging

## Create the root configuration instance

The following code will create the configuration instance and define the endpoint name.

WARNING: In Versions 6 and above, the [endpoint](/nservicebus/endpoints/) name is mandatory.

snippet: create-config

## Select and configure [dependency injection](/nservicebus/dependency-injection)

This sample uses [Autofac](/nservicebus/dependency-injection/autofac.md) with a customized instance passed into NServiceBus.

snippet: container

## Select and configure [serialization](/nservicebus/serialization)

This sample uses the [XML](/nservicebus/serialization/xml.md) serializer.

snippet: serialization

## Select and configure a [transport](/transports)

This sample uses the [learning](/transports/learning/) transport.

snippet: transport

## Select and configure [persistence](/persistence)

This sample uses the [learning](/persistence/learning/) persistence.

snippet: persistence

## Start the [endpoint](/nservicebus/endpoints/)

Enable [installers](/nservicebus/operations/installers.md) and start the endpoint.

snippet: start-bus

## Shut down the [endpoint](/nservicebus/endpoints/)

The endpoint should be stopped when the process is shut down.

snippet: stop-endpoint

## Handling [critical errors](/nservicebus/hosting/critical-errors.md)

Since this sample is configured to run as a [windows service](/nservicebus/hosting/windows-service.md), the action defined when a critical error occurs is to shut down the process.

snippet: critical-errors
