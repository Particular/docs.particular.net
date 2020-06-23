---
title: Azure Functions with Azure Storage Queues
component: ASQFunctions
summary: Azure Functions Preview
related:
 - samples/previews/azure-functions
reviewed: 2020-06-18
---

Host NServiceBus endpoints with [Azure Functions](https://docs.microsoft.com/en-us/azure/azure-functions/).

## Basic Usage

### Endpoint configuration

snippet: endpoint-configuration

The endpoint is automatically configured with the endpoint name, the transport connection string, and the logger passed into the function using a static factory method provided by `ServiceBusTriggeredEndpointConfiguration.FromAttributes` method.

Alternatively, the endpoint name can be passed in manually:

snippet: alternative-endpoint-setup

### Azure Function definition

snippet: function-definition

## Configuration

### Custom diagnostics

snippet: custom-diagnostics

## Known constraints and limitations

When using Azure Functions with Azure Storage Queues, the following points must be taken into consideration:

- Endpoints cannot create their own queues or other infrastructure using installers; the infrastructure required by the endpoint to run must be created upfront. For example:
  - Queues for commands
  - Subscription records in storage for events
- The Configuration API exposes NServiceBus transport configuration options via the `configuration.Transport` method to allow customization; however, not all of the options will be applicable to execution within Azure Functions.
- When using the default recoverability or specifying custom number of immediate retries, the number of delivery attempts specified on the underlying queue or Azure Functions host must be more than then number of the immediate retries. The Azure Functions default is 5 (`DequeueCount`) for the Azure Storage Queues trigger.
- Delayed Retries are not supported.
- Saga timeouts are not supported.

## Preparing the Azure Storage account

Queues and subscriptions have to be provisioned manually.