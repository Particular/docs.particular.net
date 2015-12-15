---
title: Endpoint configuration choices
summary: Walks through the standard configuration options for hosting an endpoint.
related:
- samples/step-by-step
- nservicebus/hosting/assembly-scanning
---

Note: This sample uses the same approach as the [NServiceBus as a Windows Service](/samples/hosting/windows-service/) for a streamlined Windows Service debugging experience.


## Code walk-through

This samples walks through the most common choices you will need to make when creating your first endpoint. It will also show the configuration APIs needed to implement those choices.


## Hosting

This sample uses a dual runnable console and Windows Service for hosting. More details on this approach can be seen in [Windows Service Hosting](/nservicebus/hosting/windows-service.md), there is also [a more detailed sample](/samples/hosting/windows-service/) of this approach. For more details on other hosting options see [Hosting choice](/nservicebus/hosting).


## Configure an [Error](/nservicebus/errors) queue

When a message fails processing it will be forwarded here.

snippet:error

Note that, in Version 5 and lower, this approach uses the [IProvideConfiguration](/nservicebus/hosting/custom-configuration-providers.md) approach to programmatically override the error queue. In Version 6 an explicit API was added.


## Configure an [Audit](/nservicebus/operations/auditing.md) queue

All messages received by an endpoint will be forwarded to the audit queue.

snippet:audit

snippet:auditxml



## Select and configure [Logging](/nservicebus/logging)

Log4net is being used to route log events to the Console.

snippet:logging


## Create the root configuration instance

snippet:create-config


## Define the Endpoint Name

snippet:endpoint-name


## Select and configure a [Container](/nservicebus/containers)

Autofac is being used with a customized container instance being passed into NServiceBus.

snippet:container


## Select and configure [Serialization](/nservicebus/serialization)

snippet:serialization


## Select and configure a [Transport](/nservicebus/transports)

snippet:transport


## Enable [Sagas](/nservicebus/sagas)

snippet:sagas


## Select and configure [Persistence](/nservicebus/persistence)

snippet:persistence


## Start the Bus

Enable installers and start the bus.

snippet:start-bus


## Shut down the bus

The bus implements `IDisposable` and should be shut down when the process is shut down.

snippet:stop-endpoint


## Handling [Critical Errors](/nservicebus/hosting/critical-errors.md)

Since this sample is configured to run as a windows service, the action defined when a critical error occurs is to shut down the process.

snippet:critical-errors
