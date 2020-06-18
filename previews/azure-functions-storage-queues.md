---
title: Azure Functions with Azure Storage Queues
summary: Azure Functions Preview
related:
 - samples/previews/azure-functions
reviewed: 2020-06-18
---

## Basic Usage

## Configuration

## Known constraints and limitations

When using Azure Functions with Azure Service Bus (ASB) or Azure Storage Queues (ASQ), the following points must be taken into consideration:

- Endpoints cannot create their own queues or other infrastructure using installers; the infrastructure required by the endpoint to run must be created upfront. For example:
  - Queues for commands (ASB and ASQ)
  - Topics and subscriptions for events (ASB)
  - Subscription records in storage for events (ASQ)
- There is no support for the `SendsAtomicWithReceive` [transport transaction mode](/transports/transactions.md#transactions-transport-transaction-sends-atomic-with-receive) with Azure Service Bus.
- Supported triggers are:
  -  [`ServiceBusTrigger`](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-service-bus) for Azure Service Bus
  - [`QueueTrigger`](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-storage-queue) for Azure Storage Queues
  - Other triggers will not result in the invocation of the NServiceBus pipeline, but can be used with a send-only endpoint to translate a function invocation into a command.
- The Configuration API exposes NServiceBus transport configuration options via the `configuration.Transport` method to allow customization; however, not all of the options will be applicable to execution within Azure Functions.
- The NServiceBus `ILog` logging abstraction and the Azure Functions `ILogger` are not wired to work together.
- When using the default recoverability or specifying custom number of immediate retries, the number of delivery attempts specified on the underlying queue (ASB) or Azure Functions host (ASB) must be more than then number of the immediate retries. The Azure Functions defaults are 10 (`MaxDeliveryCount`) for the ASB trigger and 5 (`DequeueCount`) for the ASQ trigger.
- Delayed Retries are supported only with Azure Service Bus, and not with Azure Storage Queues.
- Message handlers have to be included in the the Azure Functions assembly.

## Preparing the Azure Service Bus namespace