---
title: Azure Functions with Azure Storage Queues
component: ASQFunctions
summary: Hosting NServiceBus endpoints with Azure Functions triggered by Azure Storage Queus
related:
 - samples/previews/azure-functions/storage-queues
reviewed: 2021-01-13
---

Host NServiceBus endpoints with [Azure Functions](https://docs.microsoft.com/en-us/azure/azure-functions/).

## Basic usage

### Endpoint configuration

snippet: asq-endpoint-configuration

The endpoint is automatically configured with the endpoint name, the transport connection string, and the logger passed into the function using a static factory method provided by the `ServiceBusTriggeredEndpointConfiguration.FromAttributes` method.

Alternatively, the endpoint name can be passed in manually:

snippet: asq-alternative-endpoint-setup

### Azure Function definition

snippet: asq-function-definition

### Dispatching outside a message handler

Messages can be dispatched outside a message handler in functions activated by queue- and non-queue-based triggers.

snippet: asq-static-dispatching-outside-message-handler

Note: For statically defined endpoints, dispatching outside a message handler within a non-queue-triggered function will require a separate send-only endpoint.

snippet: asq-static-trigger-endpoint

## IFunctionsHostBuilder usage

As an alternative to the configuration approach described in the previous section, an endpoint can also be configured with a static `IFunctionEndpoint` field using the `IFunctionsHostBuilder` API as described in [Use dependency injection in .NET Azure Functions](https://docs.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection). 

### Endpoint configuration

NServiceBus can be registered and configured on the host builder using the `UseNServiceBus` extension method in the startup class:

snippet: asq-function-hostbuilder

Any services registered via the `IFunctionsHostBuilder` will be available to message handlers via dependency injection. The startup class needs to be declared via the `FunctionStartup` attribute: `[assembly: FunctionsStartup(typeof(Startup))]`.

### Azure Function definition

To access `IFunctionEndpoint` from the Azure Function trigger, inject the `IFunctionEndpoint` via constructor-injection into the containing class:

snippet: asq-function-hostbuilder-trigger

### Dispatching outside a message handler

Triggering a message using HTTP function:

snippet: asq-dispatching-outside-message-handler

## Configuration

### License

The license is provided via the `NSERVICEBUS_LICENSE` environment variable, which is set via the Function settings in the Azure Portal.
Use a `local.settings.json` file for local development. In Azure, specify a Function setting using the environment variable as the key.

include: license-file-local-setting-file

### Custom diagnostics

[NServiceBus startup diagnostics](/nservicebus/hosting/startup-diagnostics.md) are disabled by default when using Azure Functions. Diagnostics can be written to the logs with the following snippet:

snippet: asq-enable-diagnostics

### Persistence

The Azure Storage Queues transport requires a persistence for pub/sub and sagas to work.

snippet: asq-enable-persistence

Endpoints that do not have sagas and do not require pub/sub can omit persistence registration using the following transport option:

snippet: asq-disable-publishing

### Error queue

For recoverability to move the continuously failing messages to the error queue rather than the Functions poison queue, the error queue must be created in advance and configured using the following API:

snippet: asq-configure-error-queue

## Known constraints and limitations

When using Azure Functions with Azure Storage Queues, the following points must be taken into consideration:

- Endpoints cannot create their own queues or other infrastructure using installers; the infrastructure required by the endpoint to run must be created in advance. For example:
  - Queues for commands
  - Subscription records in storage for events
- The Configuration API exposes NServiceBus transport configuration options via the `configuration.Transport` method to allow customization; however, not all of the options will be applicable to execution within Azure Functions.
- When using the default recoverability or specifying custom number of immediate retries, the number of delivery attempts specified on the underlying queue or Azure Functions host must be greater than the number of the immediate retries. The Azure Functions default is 5 (`DequeueCount`) for the Azure Storage Queues trigger.

### Features dependent upon delayed delivery

The delayed delivery feature of the Azure Storage Queues transport polls for the delayed messages information and must run continuously in the background. With the Azure Functions Consumption plan, this time is limited to the function [execution duration](https://docs.microsoft.com/en-us/azure/azure-functions/functions-scale#timeout) with some additional non-deterministic cool off time. Past that time, delayed delivery will not work as expected until another message to process or the Function is kept "warm".

For features that require timely execution of the delayed delivery related features, use one of the following options:
- Keep the Function warm in the Consumption plan
- Use an App Service Plan for Functions hosting
- Use a Premium plan

The following features are supported but are not guaranteed to execute timely on the Consumption plan:
  - [Saga timeouts](/nservicebus/sagas/timeouts.md)
  - [Delayed messages](/transports/azure-storage-queues/delayed-delivery.md) destined for the endpoints hosted with Azure Functions

The following features require an explicit opt-in:
  - [Delayed retries](/nservicebus/recoverability/#delayed-retries)

snippet: asq-enable-delayed-retries

## Preparing the Azure Storage account

Queues must be provisioned manually.

Subscriptions to events are created when the endpoint executes at least once. To ensure the endpoint processes all the events, subscriptions should be created manually.
