---
title: Azure Functions with Azure Storage Queues
component: ASQFunctions
summary: Azure Functions Preview
related:
 - samples/previews/azure-functions
reviewed: 2020-07-06
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

## License

To provide a license for the endpoints hosted as Azure Functions, the environment varialbe (Function setting) `NSERVICEBUS_LICENSE` should be used contain license as escaped text.
For local development, `local.settings.json` can be used. In Azure, specify a Function setting using environment variable as the key.

include: license-file-local-setting-file

### Custom diagnostics

[NServiceBus startup diagnostics](nservicebus/hosting/startup-diagnostics.md) are disabled by default when using Azure Functions. Diagnostics can be written to the logs via the following snippet:

snippet: enable-diagnostics

## Known constraints and limitations

When using Azure Functions with Azure Storage Queues, the following points must be taken into consideration:

- Endpoints cannot create their own queues or other infrastructure using installers; the infrastructure required by the endpoint to run must be created upfront. For example:
  - Queues for commands
  - Subscription records in storage for events
- The Configuration API exposes NServiceBus transport configuration options via the `configuration.Transport` method to allow customization; however, not all of the options will be applicable to execution within Azure Functions.
- When using the default recoverability or specifying custom number of immediate retries, the number of delivery attempts specified on the underlying queue or Azure Functions host must be more than then number of the immediate retries. The Azure Functions default is 5 (`DequeueCount`) for the Azure Storage Queues trigger.

### Unsupported features

The following feature are not supported:
  - [Delayed Retries](nservicebus/recoverability#delayed-retries)
  - [Saga timeouts](nservicebus/sagas/timeouts)
  - [Delayed messages](transports/azure-storage-queues/delayed-delivery) destined to endpoints hosted with Azure Functions

### Persistence

Azure Storage Queues based transport requires a persistence for pub/sub and sagas to work.

snippet: enable-persistence

Endpoints that do not have sagas and do not require pub/sub can omit persistence registration using the following transport option:

snippet: disable-publishing

## Preparing the Azure Storage account

Queues have to be provisioned manually.

Subscriptions to events will be created when the endpoint executes at least once. To ensure the endpoint processes all the events, subscriptions should be created manually.