---
title: Azure Functions with Azure Storage Queues
component: ASQFunctions
summary: Hosting NServiceBus endpoints with Storage Queues triggered Azure Functions
related:
 - samples/previews/azure-functions/storage-queues
reviewed: 2020-07-09
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

### License

The license can be provided via the `NSERVICEBUS_LICENSE` environment variable, which can be set via the Function settings in the Azure Portal.
For local development, `local.settings.json` can be used. In Azure, specify a Function setting using the environment variable as the key.

include: license-file-local-setting-file

### Custom diagnostics

[NServiceBus startup diagnostics](/nservicebus/hosting/startup-diagnostics.md) are disabled by default when using Azure Functions. Diagnostics can be written to the logs via the following snippet:

snippet: enable-diagnostics

### Persistence

Azure Storage Queues based transport requires a persistence for pub/sub and sagas to work.

snippet: enable-persistence

Endpoints that do not have sagas and do not require pub/sub can omit persistence registration using the following transport option:

snippet: disable-publishing

### Error queue

For recoverability to move the continuously failing messages to the error queue rather than the Functions poison queue, the error queue needs to be created upfront and configured using the following API:

snippet: configure-error-queue

## Known constraints and limitations

When using Azure Functions with Azure Storage Queues, the following points must be taken into consideration:

- Endpoints cannot create their own queues or other infrastructure using installers; the infrastructure required by the endpoint to run must be created upfront. For example:
  - Queues for commands
  - Subscription records in storage for events
- The Configuration API exposes NServiceBus transport configuration options via the `configuration.Transport` method to allow customization; however, not all of the options will be applicable to execution within Azure Functions.
- When using the default recoverability or specifying custom number of immediate retries, the number of delivery attempts specified on the underlying queue or Azure Functions host must be more than then number of the immediate retries. The Azure Functions default is 5 (`DequeueCount`) for the Azure Storage Queues trigger.

### Features dependant upon delayed delivery

The delayed delivery feature of the Azure Storage Queues transport polls for the delayed messages information and needs to run in the background continuously. With the Azure Functions Consumption plan, this time is limited to the function [execution duration](https://docs.microsoft.com/en-us/azure/azure-functions/functions-scale#timeout) with some additional non-deterministic cool off time. Past that time, delayed delivery will not work as expected until another message to process or the Function is kept "warm".

For features that require timely execution of the delayed delivery related features, the following options are recommended:
- Keep the Function warm in Consumption plan
- Use App Service Plan for Functions hosting
- Use Premium plan

The following features are supported but are not guaranteed to execute timely on the Consumption plan:
  - [Saga timeouts](/nservicebus/sagas/timeouts.md)
  - [Delayed messages](/transports/azure-storage-queues/delayed-delivery.md) destined for the endpoints hosted with Azure Functions

The following features require an explicit opt-in:
  - [Delayed Retries](/nservicebus/recoverability/#delayed-retries)

snippet: enable-delayed-retries

## Preparing the Azure Storage account

Queues have to be provisioned manually.

Subscriptions to events will be created when the endpoint executes at least once. To ensure the endpoint processes all the events, subscriptions should be created manually.
