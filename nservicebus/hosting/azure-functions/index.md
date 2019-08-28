---
title: Azure Functions
summary: Hosting NServiceBus in Azure Functions
tags:
 - Hosting
related:
 - samples/azure/functions
reviewed: 2019-08-28
---

Azure Functions, and serverless computing, in general, is designed to accelerate and simplify application development. NServiceBus endpoints can be hosted in Azure Functions are subject to the constraints enforced by the Functions hosting and development model.


## Known constraints and limitations

When using Azure Functions with Azure Service Bus (ASB) or Azure Storage Queues (ASQ) the following needs to be taken into consideration:

- Endpoints cannot create their own infrastructure using installers. The infrastructure required by the endpoint to run needs to be created upfront.
  - Queues for commands (ASB and ASQ)
  - Topics and subscriptions for events (ASB)
  - Subscription records in storage for events (ASQ)
- No support for `SendsAtomicWithReceive` [transport transaction mode](/transports/transactions.md#transactions-transport-transaction-sends-atomic-with-receive) with Azure Service Bus
- Triggers that can currently be used out of the box with NServiceBus are [`ServiceBusTrigger`](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-service-bus) and [`QueueTrigger`](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-storage-queue). Other triggers will not result handlers invocation, but could be used to act as send-only endpoints to package function invocation into a command.
- Configuration API exposes transport configuration options via `configuration.Transport` property that are not all applicable to the endpoints hosted in Azure Functions.
- NServiceBus `ILog` and Azure Functions `ILogger` are not wired to work together.
- While the Functions runtime uses the default connection string in the app setting that is named "AzureWebJobsServiceBus" (ASB) or "AzureWebJobsStorage" (ASQ), there's a [bug](https://github.com/Azure/azure-functions-servicebus-extension/issues/7) with the Service Bus and the trigger connection has to be specified.
- When using the default recoverability or specifying custom number of immediate retries, the number of delivery attempts specified on the underlying queue (ASB) or Functions host (ASB) must be more than then number of the immediate retries. The Functions defaults are 10 (`MaxDeliveryCount`) for the ASB trigger and 5 (`DequeueCount`) for the ASQ trigger.